using WorkerEventHubsCopa2022;
using WorkerEventHubsCopa2022.Data;
using Microsoft.ApplicationInsights.DependencyCollector;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton<VotacaoRepository>();
        services.AddHostedService<Worker>();

        services.AddApplicationInsightsTelemetryWorkerService(options =>
        {
            options.ConnectionString =
                hostContext.Configuration.GetConnectionString("ApplicationInsights");
        });

        services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>(
        (module, o) =>
        {
            module.EnableSqlCommandTextInstrumentation = true;
        });
    })
    .Build();

host.Run();