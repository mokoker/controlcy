using Common.DTOs;
using Common.DTOs.Enums;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ControlcyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScanController : ControllerBase
    {

        public ScanController()
        {
     
        }
        // GET: api/<ScanController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ScanController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ScanController>
        [HttpPost]
        public void Post([FromBody] string value)
        {

            

            Console.WriteLine("Press any key to exit...");

        }

        // PUT api/<ScanController>/5
        [HttpPost]
        [Route("/queue")]
        public void PostQueue()
        {

            var factory = new ConnectionFactory() { HostName = "rabbitmq" ,UserName="user",Password ="pass" };
            using (var connection = factory.CreateConnection())
                
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "crawled",
                                durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

            }
        }

        [HttpPost]
        [Route("/addQueue")]
        public void AddQueue([FromBody] CrawlData status)
        {

            var factory = new ConnectionFactory() { HostName = "rabbitmq", UserName = "user", Password = "pass" };
            using (var connection = factory.CreateConnection())

            using (var channel = connection.CreateModel())
            {
                byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(status));
                channel.BasicPublish("", "crawled", null, messageBodyBytes);

            }
        }


        [HttpPost]
        [Route("/testDbWriter")]
        public void TestWriter()
        {

            var factory = new ConnectionFactory() { HostName = "rabbitmq", UserName = "user", Password = "pass" };
            using (var connection = factory.CreateConnection())

            using (var channel = connection.CreateModel())
            {
                for (int i = 0; i < 100; i++)
                {
                    var data = new CrawlData { IPAddress = "192.168.1." + i.ToString() };
                    data.Ports.Add(new PortData { Port = (ushort)i, Status = PortStatus.open, TcpUdp = TcpUdp.tcp });

                    byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
                    channel.BasicPublish("", "crawled", null, messageBodyBytes);
                }
            }
        }



        // DELETE api/<ScanController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
