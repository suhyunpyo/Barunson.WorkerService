using Azure.Identity;
using Azure.Storage.Blobs;
using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.DBModels.BarShop;
using Barunson.WorkerService.Common.Jobs;
using Barunson.WorkerService.Common.Services;
using Barunson.WorkerService.CommonBatchJob.Models;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barunson.WorkerService.CommonBatchJob.Jobs
{
    /// <summary>
    /// Memplus 맴버십, 매일 4:20
    /// </summary>
    internal class MemPlusMember: BaseJob
    {
        private readonly BlobServiceClient blobServiceClient;
        private static string StorageContainerName => "memplus";

        public MemPlusMember(ILogger<Worker> logger, IServiceProvider services, BarShopContext barShopContext,
           TelemetryClient tc, IMailSendService mail, string workerName,
           IConfiguration appconfig)
          : base(logger, services, barShopContext, tc, mail, workerName, "MemPlusMember", "20 4 * * *")
        {
            blobServiceClient = new BlobServiceClient(new Uri(appconfig.GetConnectionString("PublicStorageUrl")), new DefaultAzureCredential());
        }

        public override async Task Excute(CancellationToken cancellationToken)
        {
            try
            {
                if (!await IsExecute(cancellationToken))
                    return;
                _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {WorkerName}-{funcName} is working.");
                var targetDate = DateTime.Today;

                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<BarShopContext>();

                    // 일일 회원 데이터 생성
                    await context.Database.ExecuteSqlRawAsync("exec sp_memplus_daily", cancellationToken);
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
