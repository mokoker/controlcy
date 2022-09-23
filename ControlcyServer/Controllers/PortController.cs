using Common.Database.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControlcyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortController : ControllerBase
    {
        private ControlcyDbContext _context;
        public PortController(ControlcyDbContext context)
        {
            _context = context;

        }
        [HttpGet("/{port}")]
        public List<string> Get20(ushort port)
        {
            List<string> result = new List<string>();
            var scan = _context.Ports.Include(z => z.Scan).OrderByDescending(x => x.Scan.Id).Where(y => y.PortNumber == port).Take(20);
            var target = scan.Select(x => x.Scan.AddressIp).ToList();
            return target;
        }


    }
}
