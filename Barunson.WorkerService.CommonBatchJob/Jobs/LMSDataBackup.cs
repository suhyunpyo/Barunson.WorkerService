using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.Jobs;
using Barunson.WorkerService.Common.Services;
using Microsoft.ApplicationInsights;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Barunson.WorkerService.CommonBatchJob.Jobs
{
    /// <summary>
    /// LMS 발송 데이터 6개월 삭제 건 보존을 위한 데이터 백업
    /// 매월 첫 일요일 오전 3시
    /// </summary>
    internal class LMSDataBackup : BaseJob
    {
        public LMSDataBackup(ILogger<Worker> logger, IServiceProvider services, BarShopContext barShopContext,
            TelemetryClient tc, IMailSendService mail, string workerName)
            : base(logger, services, barShopContext, tc, mail, workerName, "LMSDataBackup", "0 3 * * 0#1")
        { }

        public override async Task Excute(CancellationToken cancellationToken)
        {
            try
            {
                if (!await IsExecute(cancellationToken))
                    return;

                _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is working.");

                var Now = DateTime.Now;

                //11개월 전 데이터 부터 1개월 전까지
                var month = new DateTime(Now.Year, Now.Month, 1);
                var frommonth = month.AddMonths(-11);  //테이블이 월단위이기 때문에 12로 하면 당월이 포함 됨어 문제 발생.
                var tomonth = month.AddMonths(-1);
                var targetMonth = frommonth;

                using (var fncScope = _serviceProvider.CreateScope())
                {
                    var moContext = fncScope.ServiceProvider.GetRequiredService<MoSvrContext>();
                    int cnt = 0;

                    while (targetMonth <= tomonth)
                    {
                        using (var trans = await moContext.Database.BeginTransactionAsync(cancellationToken))
                        {
                            var timeParam = new SqlParameter("@intime", System.Data.SqlDbType.VarChar, 16);
                            timeParam.Value = targetMonth.ToString("yyyyMMddHHmmss");  //20210601133556

                            //SMS
                            var smsTableName = $"T_SMS_HIST_RV_{targetMonth:MM}";
                            //TagetMonth 보다 작은 IN_TIME 데이터 삭제
                            var smsDelCmdText = $"Delete From {smsTableName} Where IN_TIME < @intime";
                            cnt = await moContext.Database.ExecuteSqlRawAsync(smsDelCmdText, timeParam);
                            _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName}, Deleted rows {cnt} in table {smsTableName}.");

                            //데이터 백업
                            var smsBackupText = $"INSERT INTO T_SMS_HIST_RV SELECT * from {smsTableName} a Where Not Exists(Select * From T_SMS_HIST_RV as b Where a.MSG_KEY = b.MSG_KEY and a.IN_TIME = b.IN_TIME)";
                            cnt = await moContext.Database.ExecuteSqlRawAsync(smsBackupText, cancellationToken);
                            _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName}, Backup rows {cnt} in table {smsTableName}.");


                            //MMS
                            var mmsTableName = $"T_MMS_HIST_RV_{targetMonth:MM}";
                            //TagetMonth 보다 작은 IN_TIME 데이터 삭제
                            var mmsDelCmdText = $"Delete From {mmsTableName} Where IN_TIME < @intime";
                            cnt = await moContext.Database.ExecuteSqlRawAsync(mmsDelCmdText, timeParam);
                            _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName}, Deleted rows {cnt} in table {mmsTableName}.");

                            //데이터 백업
                            var mmsBackupText = $"INSERT INTO T_MMS_HIST_RV SELECT * from {mmsTableName} a Where Not Exists(Select * From T_MMS_HIST_RV as b Where a.MSG_KEY = b.MSG_KEY and a.IN_TIME = b.IN_TIME)";
                            cnt = await moContext.Database.ExecuteSqlRawAsync(mmsBackupText, cancellationToken);
                            _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName}, Backup rows {cnt} in table {mmsTableName}.");

                            await trans.CommitAsync(cancellationToken);
                        }

                        targetMonth = targetMonth.AddMonths(1);
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
