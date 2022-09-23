using System.Diagnostics;
using Common.Helper;
using RestSharp;

namespace AgentService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _provider;
        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _provider = serviceProvider;
        }

      

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            Task[] taskArray = new Task[5];
            try
            {
                for (int i = 0; i < taskArray.Length; i++)
                {
                    try
                    {
                        taskArray[i] = Task.Factory.StartNew(() =>
                        {
                            var serivce = _provider.GetService<MapWorker>();
                            serivce.Run();
                        });
                    }
                    catch (Exception ae)
                    {
                        Console.WriteLine("Stop the work and notify others :" + ae.Message);
                    }
                };
            }
            catch (Exception ex)
            {

            }



            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(10000, stoppingToken);
            }
        }


    }
}