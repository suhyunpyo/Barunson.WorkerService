using Barunson.WorkerService.Common;
using Barunson.WorkerService.MCardResourceCleanJob;

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
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();

