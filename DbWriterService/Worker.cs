namespace DbWriterService
{

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private DbWriterJob _job;
        public Worker(ILogger<Worker> logger,DbWriterJob job)
        {
            _logger = logger;
            _job = job;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task[] taskArray = new Task[2];
            try
            {
                for (int i = 0; i < taskArray.Length; i++)
                {
                    try
                    {
                        taskArray[i] = Task.Factory.StartNew(() =>
                        {
                            _job.Run();
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
                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}