using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DTOs;
using DbWriterService.Database;
using DbWriterService.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DbWriterService
{

    public class DbWriterJob
    {

        private ILogger<DbWriterJob> _logger;
        private IConfiguration _configuration;


        public DbWriterJob(ILogger<DbWriterJob> logger,IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void Run()
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = "rabbitmq", UserName = "user", Password = "pass" };

                IConnection _connection = factory.CreateConnection();
                IModel _channel = _connection.CreateModel();
                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (model, ea) =>
                {
                    ControlcyDbContext context = new ControlcyDbContext(_configuration);
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var status =  JsonConvert.DeserializeObject<CrawlData>(message);
                    Address address;
                    var existing_address = context.Addresses.Find(status.IPAddress);
                    if(existing_address == null)
                        address  = new Address { Ip = status.IPAddress};
                    else
                        address = existing_address;

                    var scan = new Scan { Address=address,ScanDate = DateTime.UtcNow };

                    scan.Ports = new List<Port>();
                    foreach (PortData x in status.Ports)
                    {
                        scan.Ports.Add(new Port { PortNumber = x.Port, Status = x.Status });
                    }
                    context.Scans.Add(scan);

                    context.SaveChanges();

                };
                _channel.BasicConsume(queue: "crawled",
                                   autoAck: true,
                                   consumer: consumer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

    }
}
