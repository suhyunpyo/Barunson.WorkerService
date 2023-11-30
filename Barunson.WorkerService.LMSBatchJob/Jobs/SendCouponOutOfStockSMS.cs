using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.Services;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.LMSBatchJob.Jobs
{
    /// <summary>
    /// 바른손,제휴 150 미만 / 비핸즈, 더카드, 프페 100 미만인 경우 확인등록문자 보냄
    /// 담당자 휴대폰 번호가 하드 코딩 되어 있어, 기존 Sp 호출 유지 함.
    /// </summary>
    internal class SendCouponOutOfStockSMS: LMSBaseJob
    {
        public SendCouponOutOfStockSMS(ILogger logger, IServiceProvider services, BarShopContext taskContext, TelemetryClient tc, IMailSendService mail, ILMSSendService mms
            , string workerName)
            : base(logger, services, taskContext, tc, mail, mms, workerName, "SendCouponOutOfStockSMS", "30 9 * * *")
        {
        }

        public override async Task Excute(CancellationToken cancellationToken)
        {
            try
            {
                if (!await IsExecute(cancellationToken))
                    return;

                _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is working.");
                               

                using (var fncScope = _serviceProvider.CreateScope())
                {
                    var barshopContext = fncScope.ServiceProvider.GetRequiredService<BarShopContext>();
                    await barshopContext.Database.ExecuteSqlRawAsync("exec [dbo].[SP_WEDDING_COUPON_SMS]", cancellationToken);
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
