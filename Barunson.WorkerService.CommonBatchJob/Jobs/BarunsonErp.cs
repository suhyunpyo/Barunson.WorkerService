using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.Jobs;
using Barunson.WorkerService.Common.Services;
using Microsoft.ApplicationInsights;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Barunson.WorkerService.CommonBatchJob.Jobs
{
    /// <summary>
    /// 바른손 Shop ERPDB 전송, 기존 SP 사용, 매일 오전 1:30
    /// </summary>
    internal class BarunsonErp : BaseJob
    {

        public BarunsonErp(ILogger<Worker> logger, IServiceProvider services, BarShopContext barShopContext,
            TelemetryClient tc, IMailSendService mail, string workerName)
            : base(logger, services, barShopContext, tc, mail, workerName, "BarunsonErp", "30 2 * * *")
        { }
        public override async Task Excute(CancellationToken cancellationToken)
        {
            try
            {
                if (!await IsExecute(cancellationToken))
                    return;
                _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is working.");
                var Now = DateTime.Now;
                var fdate = Now.Date.AddDays(-14).ToString("yyyyMMdd");
                var tdate = Now.Date.AddDays(-1).ToString("yyyyMMdd");

                using (var fncScope = _serviceProvider.CreateScope())
                {
                    var barshopContext = fncScope.ServiceProvider.GetRequiredService<BarShopContext>();
                    using (var command = (SqlCommand)barshopContext.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandTimeout = (int)TimeSpan.FromMinutes(20).TotalSeconds;

                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SDate", fdate);
                        command.Parameters.AddWithValue("@EDate", tdate);

                        await barshopContext.Database.OpenConnectionAsync(cancellationToken);

                        command.CommandText = "sp_ERP_Transfer_Daeri";
                        await command.ExecuteNonQueryAsync();

                        command.CommandText = "sp_ERP_Transfer_Daeri_New";
                        await command.ExecuteNonQueryAsync();

                        command.CommandText = "SP_ERP_TRANSFER_SHOP_BD";
                        await command.ExecuteNonQueryAsync();

                        command.CommandText = "sp_ERP_Transfer_Shop_New";
                        await command.ExecuteNonQueryAsync();

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
