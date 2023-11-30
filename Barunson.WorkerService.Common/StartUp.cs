using Azure.Identity;
using Barunson.WorkerService.Common.DBContext;
using Barunson.WorkerService.Common.Models;
using Barunson.WorkerService.Common.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SendGrid.Extensions.DependencyInjection;
using System.Text.Json;

namespace Barunson.WorkerService.Common
{
    public static class StartUp
    {
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, builder) =>
                {
                    builder.AddAzureKeyVault(
                        new Uri($"https://barunsecret.vault.azure.net/"),
                        new DefaultAzureCredential());
                })
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;

                    #region DB Context 

                    services.AddDbContext<BarunsonContext>(options =>
                         options.UseSqlServer(configuration.GetConnectionString("BarunsonDBConn")));
                    services.AddDbContext<BarShopContext>(options =>
                         options.UseSqlServer(configuration.GetConnectionString("BarShopDBConn")));
                    services.AddDbContext<MoSvrContext>(options =>
                         options.UseSqlServer(configuration.GetConnectionString("MoSvrDBConn")));
                    services.AddDbContext<DearDeerContext>(options =>
                        options.UseMySql(
                            configuration.GetConnectionString("DearDeerDBConn"), 
                            new MariaDbServerVersion(new Version(10, 1))
                            )
                        );

                    #endregion

                    #region Config Settings & DI
                    services.AddSingleton<IConfiguration>(configuration);

                    var pginfostr = configuration.GetSection("PgMertInfos").Get<string>();
                    if (!string.IsNullOrEmpty(pginfostr))
                        services.AddSingleton<List<PgMertInfo>>(JsonSerializer.Deserialize<List<PgMertInfo>>(pginfostr));
                    else
                        services.AddSingleton<List<PgMertInfo>>(configuration.GetSection("PgMertInfos").Get<List<PgMertInfo>>());

                    var mailOption = configuration.GetSection("MailServer").Get<MailServerOption>();
                    services.AddSingleton(mailOption);
                    services.AddSendGrid(options =>
                        options.ApiKey = mailOption.Password
                    );
                                       
                    var testOption = configuration.GetSection("TestData").Get<TestDataOption>();
                    services.AddSingleton(testOption);

                    services.AddSingleton<IMailSendService, MailSendService>();
                    services.AddSingleton<ILMSSendService, LMSSendService>();

                    services.AddHttpClient();
                    #endregion

                    services.AddLogging();
                    services.AddApplicationInsightsTelemetryWorkerService();
                    services.AddApplicationInsightsTelemetryProcessor<NoSQLDependencies>();

                });

    }
}
