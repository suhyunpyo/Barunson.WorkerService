using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.Jobs;
using Barunson.WorkerService.Common.Services;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.CommonBatchJob.Jobs
{
    /// <summary>
    /// [메인] 혜택배너 (417)  - 진행 또는 대기상태 리스트만 기간이 지나면 종료처리
    ///  대체배너는 상시임. 따라서 종료대상이 아니다.
    ///  현재 비핸즈 메인만 해당
    ///  매일 오전 0:30 
    /// </summary>
    internal class MainBenefitBanner : BaseJob
    {

        public MainBenefitBanner(ILogger<Worker> logger, IServiceProvider services, BarShopContext barShopContext,
            TelemetryClient tc, IMailSendService mail, string workerName)
            : base(logger, services, barShopContext, tc, mail, workerName, "MainBenefitBanner", "30 0 * * *")
        { }
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
                    var barshopContext = fncScope.ServiceProvider.GetRequiredService<BarShopContext>();
                    var targetDate = Now.Date.ToString("yyyy-MM-dd");

                    using (var trans = await barshopContext.Database.BeginTransactionAsync(cancellationToken))
                    {

                        var query = from m in barshopContext.BENEFIT_BANNER
                                    where m.DISPLAY_YN == "Y" && m.END_YN == "N"
                                        && (m.B_TYPE_NO == 1 || m.B_TYPE_NO == 2)
                                        && m.EVENT_E_DT.CompareTo(targetDate) < 0
                                    select m;
                        var items = await query.ToListAsync(cancellationToken);

                        foreach (var item in items)
                        {
                            item.END_YN = "Y";
                            item.DISPLAY_YN = "N";
                            item.UPDATED_DATE = Now;
                            item.UPDATED_UID = "BATCH";
                        }
                        await barshopContext.SaveChangesAsync(cancellationToken);
                        await trans.CommitAsync(cancellationToken);
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
    }
}
