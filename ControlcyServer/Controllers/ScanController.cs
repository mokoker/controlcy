using Common.Database.Database;
using Common.DTOs;
using Common.DTOs.Enums;
using ControlcyServer.Database;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ControlcyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]

    public class ScanController : ControllerBase
    {
        private ControlcyDbContext _context;
        public ScanController(ControlcyDbContext context)
        {
            _context = context;

        }

        // GET api/<ScanController>/5
        [HttpGet("/one/{ip}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CrawlData))]

        public IActionResult GetCrawlData(string ip)
        {
            var scan = _context.Scans.Include(z => z.Ports).OrderByDescending(x => x.Id).FirstOrDefault(y => y.AddressIp == ip);
            if (scan == null)
                return NotFound();
            return Ok(scan.GetCrawlData());
        }

        [HttpGet("/all/{ip}")]
        public List<CrawlData> GetAllCrawlData(string ip)
        {
            var scan = _context.Scans.Include(z => z.Ports).OrderByDescending(x => x.Id).Where(y => y.AddressIp == ip);
            if (scan.Count() > 0)
            {
                var target = scan.Select(x => x.GetCrawlData()).ToList();
                return target;
            }
            return new List<CrawlData>();

        }

        //[HttpPost]
        //[Route("/queue")]
        //public void PostQueue()
        //{

        //    var factory = new ConnectionFactory() { HostName = "rabbitmq", UserName = "user", Password = "pass" };
        //    using (var connection = factory.CreateConnection())

        //    using (var channel = connection.CreateModel())
        //    {
        //        channel.QueueDeclare(queue: "crawled",
        //                        durable: true,
        //                        exclusive: false,
        //                        autoDelete: false,
        //                        arguments: null);

        //    }
        //}
        /*
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
        */

        //[HttpPost]
        //[Route("/testDbWriter")]
        //public void TestWriter()
        //{

        //    var factory = new ConnectionFactory() { HostName = "rabbitmq", UserName = "user", Password = "pass" };
        //    using (var connection = factory.CreateConnection())

        //    using (var channel = connection.CreateModel())
        //    {
        //        for (int i = 0; i < 100; i++)
        //        {
        //            var data = new CrawlData { IPAddress = "192.168.1." + i.ToString() };
        //            data.Ports.Add(new PortData { Port = (ushort)i, Status = PortStatus.open, TcpUdp = TcpUdp.tcp });

        //            byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
        //            channel.BasicPublish("", "crawled", null, messageBodyBytes);
        //        }
        //    }
        //}
    }
}
