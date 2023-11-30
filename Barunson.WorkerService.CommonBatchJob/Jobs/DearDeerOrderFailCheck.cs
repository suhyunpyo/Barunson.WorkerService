using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.Jobs;
using Barunson.WorkerService.Common.Services;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Barunson.WorkerService.CommonBatchJob.Jobs
{
    /// <summary>
    /// DearDeer 주문 오류, 매시 20분 간격
    /// </summary>
    internal class DearDeerOrderFailCheck : BaseJob
    {

        public DearDeerOrderFailCheck(ILogger<Worker> logger, IServiceProvider services, BarShopContext barShopContext,
            TelemetryClient tc, IMailSendService mail, string workerName)
            : base(logger, services, barShopContext, tc, mail, workerName, "DearDeerOrderFailCheck", "20 */1 * * *")
        { }

        public override async Task Excute(CancellationToken cancellationToken)
        {
            try
            {
                if (!await IsExecute(cancellationToken))
                    return;
                _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is working.");
                var Now = DateTime.Now;

                var mailSubject = "[디얼디어]주문 빠른손 연동 오류";

                var mailBody = new StringBuilder();
                mailBody.AppendLine("<table cellpadding=\"0\" cellspacing =\"0\" width=\"100%\">");
                mailBody.AppendLine("<tr>");
                mailBody.AppendLine($"<td>Barun Order seq</td>");
                mailBody.AppendLine($"<td>DD Order No</td>");
                mailBody.AppendLine($"<td>Order Date</td>");
                mailBody.AppendLine($"<td>User ID</td>");
                mailBody.AppendLine($"<td>User Name</td>");
                mailBody.AppendLine($"<td>Message</td>");
                mailBody.AppendLine("</tr>");
                bool hasFailed = false;

                var targetDate = Now.AddHours(-2);

                using (var fncScope = _serviceProvider.CreateScope())
                {
                    var barshopContext = fncScope.ServiceProvider.GetRequiredService<BarShopContext>();
                    var ddContext = fncScope.ServiceProvider.GetRequiredService<DearDeerContext>();

                    //바른손 custom_order_WeddInfo 누락 검색
                    var bQuery = from a in barshopContext.custom_order
                                 where a.order_date > targetDate
                                 && a.sales_Gubun == "SD"
                                 && a.up_order_seq == null
                                 && !barshopContext.custom_order_WeddInfo.Any(b => b.order_seq == a.order_seq)
                                 select new
                                 {
                                     a.order_date,
                                     a.order_seq,
                                     a.member_id,
                                     a.order_name,
                                     a.order_email
                                 };
                    var bOrderItems = await bQuery.ToListAsync(cancellationToken);
                    if (bOrderItems.Count > 0)
                    {
                        hasFailed = true;
                        foreach (var item in bOrderItems)
                        {
                            //디디 주문 번호 추가
                            var ddQuery = from a in ddContext.orders
                                          where a.barunson_order_seq == item.order_seq
                                          select a.order_no;
                            var ddOrderNo = await ddQuery.FirstOrDefaultAsync(cancellationToken);

                            mailBody.AppendLine("<tr>");
                            mailBody.AppendLine($"<td>{item.order_seq}</td>");
                            mailBody.AppendLine($"<td>{ddOrderNo}</td>");
                            mailBody.AppendLine($"<td>{item.order_date?.ToString("yyyy-MM-dd HH:mm:ss")}</td>");
                            mailBody.AppendLine($"<td>{item.member_id}</td>");
                            mailBody.AppendLine($"<td>{item.order_name}</td>");
                            mailBody.AppendLine($"<td>custom_order_WeddInfo 누락</td>");
                            mailBody.AppendLine("</tr>");
                        }
                    }

                    //DD에서 바른손 주문 번호 누락 검색
                    var dQuery = from a in ddContext.orders
                                 where a.created_at > targetDate
                                 && a.barunson_status_seq == 1
                                 && a.barunson_order_flag == "T"
                                 && a.barunson_order_seq == null
                                 select new
                                 {
                                     a.order_no,
                                     a.created_at,
                                     a.user_id
                                 };
                    var dOrderItems = await dQuery.ToListAsync(cancellationToken);
                    if (dOrderItems.Count > 0)
                    {
                        hasFailed = true;
                        foreach (var item in dOrderItems)
                        {
                            mailBody.AppendLine("<tr>");
                            mailBody.AppendLine($"<td></td>");
                            mailBody.AppendLine($"<td>{item.order_no}</td>");
                            mailBody.AppendLine($"<td>{item.created_at?.ToString("yyyy-MM-dd HH:mm:ss")}</td>");
                            mailBody.AppendLine($"<td>{item.user_id}</td>");
                            mailBody.AppendLine($"<td></td>");
                            mailBody.AppendLine($"<td>orders table: barunson_order_seq 누락</td>");
                            mailBody.AppendLine("</tr>");
                        }
                    }
                    //DD에서 바른손 셈플 주문 번호 누락 검색
                    var dsQuery = from a in ddContext.sample_orders
                                  where a.created_at > targetDate
                                  && a.order_state == "E"
                                  && a.barunson_order_seq == null
                                  select new
                                  {
                                      a.sample_order_no,
                                      a.created_at,
                                      a.user_id
                                  };
                    var dsOrderItems = await dsQuery.ToListAsync(cancellationToken);
                    if (dsOrderItems.Count > 0)
                    {
                        hasFailed = true;
                        foreach (var item in dsOrderItems)
                        {
                            mailBody.AppendLine("<tr>");
                            mailBody.AppendLine($"<td></td>");
                            mailBody.AppendLine($"<td>{item.sample_order_no}</td>");
                            mailBody.AppendLine($"<td>{item.created_at?.ToString("yyyy-MM-dd HH:mm:ss")}</td>");
                            mailBody.AppendLine($"<td>{item.user_id}</td>");
                            mailBody.AppendLine($"<td></td>");
                            mailBody.AppendLine($"<td>sample orders table: barunson_order_seq 누락</td>");
                            mailBody.AppendLine("</tr>");
                        }
                    }
                }

                mailBody.AppendLine("</table>");

                if (hasFailed)
                    await _mail.SendAsync(mailSubject, mailBody.ToString());

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
