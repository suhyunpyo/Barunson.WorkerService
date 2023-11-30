using Barunson.WorkerService.Common;
using Barunson.WorkerService.Common.Models;
using Barunson.WorkerService.CommonBatchJob;
using Barunson.WorkerService.CommonBatchJob.Config;
using System.Text.Json;

var hostbuilder = StartUp.CreateHostBuilder(args);

IHost host = hostbuilder
    .ConfigureAppConfiguration((hostContext, builder) =>
    {
        if (hostContext.HostingEnvironment.IsDevelopment())
        {
            builder.AddUserSecrets<Program>();
        }
    })
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;

        var smbUser = configuration.GetSection("SMBUser").Get<SMBUser>();
        services.AddSingleton(smbUser);
        var cjConfig = configuration.GetSection("CJLogisticsAPIConfig").Get<CJLogisticsAPIConfig>();
        services.AddSingleton(cjConfig);
        var kakoinfo = configuration.GetSection("KakaoBank").Get<KakaoBankConfig>();
        services.AddSingleton(kakoinfo);

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
