using DbWriterService;
using DbWriterService.Database;

try
{
    IHost host = Host.CreateDefaultBuilder(args)
        .ConfigureServices(services =>
        {
            services.AddHostedService<Worker>();
            services.AddTransient<DbWriterJob>();
            services.AddDbContext<ControlcyDbContext>(ServiceLifetime.Transient);
        })
        .Build();
    await host.RunAsync();
}
catch (Exception ex)
{

}

