using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.Services;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.LMSBatchJob.Jobs
{
    /// <summary>
    /// 청첩장 발송완료일 3일 후인 SMS 수신 동의 고객 발송
    /// </summary>
    internal class SendOrderReview : LMSBaseJob
    {
        public SendOrderReview(ILogger<Worker> logger, IServiceProvider services, BarShopContext taskContext,
           TelemetryClient tc, IMailSendService mail, ILMSSendService mms, string workerName)
           : base(logger, services, taskContext, tc, mail, mms, workerName, "SendOrderReview", "0 17 * * *")
        { }

        public override async Task Excute(CancellationToken cancellationToken)
        {
            try
            {
                if (!await IsExecute(cancellationToken))
                    return;

                _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is working.");

                var Now = DateTime.Now;
                var fromDt = Now.AddDays(-4).Date;
                var toDt = Now.AddDays(-3).Date;

                //Template Code
                var templateCodes = new Dictionary<string, string>()
                    {
                        {"SB", "BH0134" },
                        {"ST", "thebiz081" },
                        {"SS", "P050"},
                        {"B", "BM0155"}
                    };

                var sendModels = new List<BizTalkModel>();

                using (var fncScope = _serviceProvider.CreateScope())
                {
                    var barshopContext = fncScope.ServiceProvider.GetRequiredService<BarShopContext>();

                    // 발송 비즈톡 정보
                    var bizTemplateQ = from m in barshopContext.wedd_biztalk
                                       where m.USE_YORN == "Y"
                                       && templateCodes.Keys.ToList().Contains(m.sales_gubun)
                                       && templateCodes.Values.ToList().Contains(m.template_code)
                                       select m;
                    var bizTemplateList = await bizTemplateQ.ToListAsync(cancellationToken);

                    //발송대상
                    var query = from m in barshopContext.custom_order
                                join u in barshopContext.VW_USER_INFO on
                                    new { uid = m.member_id, site = (m.sales_Gubun == "C" || m.sales_Gubun == "H") ? "B" : m.sales_Gubun }
                                    equals new { uid = u.uid, site = u.site_div }
                                where m.status_seq == 15 && !string.IsNullOrEmpty(m.member_id) && !string.IsNullOrEmpty(m.sales_Gubun) && u.chk_sms == "Y"
                                && m.src_send_date > fromDt && m.src_send_date < toDt
                                && m.order_hphone.Length == 13
                                select new
                                {
                                    u.site_div,
                                    m.order_hphone,
                                    m.order_name,
                                    m.company_seq
                                };
                    var items = await query.ToListAsync(cancellationToken);

                    foreach (var item in items)
                    {
                        if (!templateCodes.ContainsKey(item.site_div))
                            continue;

                        var bizTemplate = bizTemplateList.FirstOrDefault(m => m.sales_gubun == item.site_div && m.template_code == templateCodes[item.site_div]);
                        if (bizTemplate == null)
                            continue;

                        var message = bizTemplate.content.Replace("#{name}", item.order_name.Trim());
                        sendModels.Add(new BizTalkModel
                        {
                            SendTime = DateTime.Now,
                            Subject = bizTemplate.lms_subject,
                            Message = message,
                            CallBack = bizTemplate.callback,
                            RecipientNum = item.order_hphone,
                            MessageType = (BizTalkMsgType)bizTemplate.msg_type,
                            Senderkey = bizTemplate.sender_key,
                            TemplateCode = bizTemplate.template_code,
                            KKOBtnType = bizTemplate.kko_btn_type,
                            KKOBtnInfo = bizTemplate.kko_btn_info,
                            EtcText1 = item.site_div,
                            EtcText2 = $"{WorkerName}-{funcName}",
                            EtcNum1 = (item.site_div == "B") ? 5000 : item.company_seq
                        });
                    }

                }

                var success = await _mms.SendBizTalkAsync(sendModels, cancellationToken);

                await SetNextTimeTaskItemAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName}, has error.");
            }

            _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is end.");
        }
    }
}
