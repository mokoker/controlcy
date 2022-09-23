using AgentService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<MapWorker>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
