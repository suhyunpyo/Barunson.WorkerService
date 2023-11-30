using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.Jobs;
using Barunson.WorkerService.Common.Services;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Barunson.WorkerService.CommonBatchJob.Jobs
{
    /// <summary>
    /// 대기 초안 취소 - 매일 오전 1:00
    /// </summary>
    internal class ChoanCancel : BaseJob
    {

        public ChoanCancel(ILogger<Worker> logger, IServiceProvider services, BarShopContext barShopContext,
            TelemetryClient tc, IMailSendService mail, string workerName)
            : base(logger, services, barShopContext, tc, mail, workerName, "ChoanCancel", "0 1 * * *")
        { }
        public override async Task Excute(CancellationToken cancellationToken)
        {
            try
            {
                if (!await IsExecute(cancellationToken))
                    return;
                _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is working.");
                
                var today = DateTime.Today;
                var now = DateTime.Now;
                var checkDate = today.AddDays(-2); //기본값 3일전 데이터를 읽음
                var orderSeq = new StringBuilder();
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<BarShopContext>();

                    var query = from o in context.custom_order
                                where o.AUTO_CHOAN_STATUS_CODE == "138003"
                                    && o.status_seq == 1
                                    && o.order_date < checkDate
                                select o;

                    var items = await query.ToListAsync(cancellationToken);
                    foreach (var item in items)
                    {
                        item.AUTO_CHOAN_STATUS_CODE = "138001";
                        item.status_seq = 3;
                        item.src_cancel_date = now;

                        orderSeq.Append(item.order_seq + ",");
                    }
                    await context.SaveChangesAsync(cancellationToken);
                    _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} " + orderSeq.ToString() + " Order canceled.");
                }

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
