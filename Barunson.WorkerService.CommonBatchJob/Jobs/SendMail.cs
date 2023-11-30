using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.Jobs;
using Barunson.WorkerService.Common.Services;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;

namespace Barunson.WorkerService.CommonBatchJob.Jobs
{
    /// <summary>
    /// 메일 발송 작업, 매 10분
    /// </summary>
    internal class SendMail : BaseJob
    {
        private readonly IHttpClientFactory _clientFactory;

        public SendMail(ILogger<Worker> logger, IServiceProvider services, BarShopContext barShopContext,
            TelemetryClient tc, IMailSendService mail, string workerName,
            IHttpClientFactory clientFactory)
            : base(logger, services, barShopContext, tc, mail, workerName, "SendMail", "*/10 * * * *")
        { 
            _clientFactory = clientFactory;
        }
        public override async Task Excute(CancellationToken cancellationToken)
        {
            try
            {
                if (!await IsExecute(cancellationToken))
                    return;
                _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is working.");
                var Now = DateTime.Now;

                using (var fncScope = _serviceProvider.CreateScope())
                {
                    var sDate = DateTime.Today.AddDays(-1);
                    var barshopContext = fncScope.ServiceProvider.GetRequiredService<BarShopContext>();

                    //발송 대상 읽기, 미발송이고 마지막 수정일이 하루전 이내 일경우만 발송
                    var targetQuery = from c in barshopContext.SendEmailContent
                                      where c.SendYn == false && c.ModDate > sDate
                                      select c;
                    var targetItems = await targetQuery.ToListAsync(cancellationToken);
                    if (targetItems.Count > 0)
                    {
                        //Mail Master 일기
                        var ecode = targetItems.Select(m => m.EmailFormCode).Distinct().ToList();
                        var emQuery = from m in barshopContext.SendEmailMaster
                                      where ecode.Contains(m.EmailFormCode)
                                      && m.UseYn == true
                                      select m;
                        var emItems = await emQuery.ToListAsync(cancellationToken);

                        var emrQuery = from m in barshopContext.SendEmailRecipient
                                       where ecode.Contains(m.EmailFormCode)
                                       && m.UserYn == true
                                       select m;
                        var emrItems = await emrQuery.ToListAsync(cancellationToken);

                        foreach (var item in targetItems)
                        {
                            var emItem = emItems.FirstOrDefault(m => m.EmailFormCode == item.EmailFormCode);
                            if (emItem == null)
                            {
                                //발송 여부가 없으면 바로 완료 처리, 발송하지 않음.
                                item.SendYn = true;
                                item.SendDate = DateTime.Now;
                            }
                            else
                            {
                                var mailSubject = emItem.Title;
                                var url = new Uri($"{emItem.Contents}?cid={item.ContentId}");
                                var mailBody = await GetSendMailBodyAsync(url, cancellationToken);
                                if (mailBody != "NoData")
                                {
                                    var fromAddr = new EmailAddress(emItem.SenderEmailAddress, emItem.SenderName);
                                    var toAddr = new List<EmailAddress>();
                                    if (!string.IsNullOrEmpty(item.ToEmailAddress))
                                        toAddr.Add(new EmailAddress(item.ToEmailAddress, item.ToName));
                                    foreach (var emritem in emrItems.Where(m => m.EmailFormCode == item.EmailFormCode))
                                    {
                                        toAddr.Add(new EmailAddress(emritem.ToEmailAddress, emritem.ToName));
                                    }

                                    var isSend = await _mail.SendAsync(mailSubject, mailBody, toAddr, null, fromAddr);
                                    if (isSend)
                                    {
                                        item.SendYn = true;
                                        item.SendDate = DateTime.Now;
                                    }
                                }
                            }
                            await barshopContext.SaveChangesAsync(cancellationToken);
                        }
                    }

                }

                await SetNextTimeTaskItemAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName}, has error.");
            }

            _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is end.");
        }

        private async Task<string> GetSendMailBodyAsync(Uri api, CancellationToken cancellationToken)
        {
            var contentString = "NoData";
            var client = _clientFactory.CreateClient();
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Get;
                    request.RequestUri = api;

                    var response = await client.SendAsync(request, cancellationToken);
                    response.EnsureSuccessStatusCode();

                    contentString = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                contentString = "NoData";
                _logger.LogError(e, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName} api call error.");
            }
            return contentString;
        }
    }
}
