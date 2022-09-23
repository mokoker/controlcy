using Common.Helper;
using ControlcyServer.Database;
using ControlcyServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControlcyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SegmentController : ControllerBase
    {
        private ManagementDbContext _context;
        private static object _locker=  new object();
        public SegmentController(ManagementDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        [Route("/Segment")]
        public void PostSegment(string cidr)
        {
            var divided = CIDRDivider.Divide(cidr);
            foreach (var segment in divided)
            {
                _context.Segments.Add(new Segment { CIDR = segment, LastScan = DateTime.MinValue });
            }
            _context.SaveChanges();
           
           
        }
        [HttpGet]
        [Route("/Segment")]
        public string GetSegment()
        {
            lock (_locker)
            {
                var yesterday = DateTime.Now.AddDays(-1);
                var x = _context.Segments.OrderBy(x=>x.LastScan).First();
                x.LastScan = DateTime.UtcNow;
                _context.SaveChanges();
                return x.CIDR;
            }


        }
    }
}
