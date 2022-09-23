using System.ComponentModel.DataAnnotations.Schema;
using Common.DTOs;

namespace DbWriterService.Models
{
    [Table("scans")]
    public class Scan
    {
        public Scan()
        {
            Ports = new List<Port>();
        }
        public int Id { get; set; }
        public DateTime ScanDate { get; set; }
        public Address Address { get; set; }
        public List<Port> Ports { get; set; }

        [ForeignKey("Address")]
        public string AddressIp { get; set; }

        public CrawlData GetCrawlData()
        {
            var x = new CrawlData();
            x.IPAddress = AddressIp;
            foreach (var port in Ports)
            {
                x.Ports.Add(port.GetPortData());
            }
            return x;
        }
    }

   
}
