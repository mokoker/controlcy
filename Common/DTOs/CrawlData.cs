using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class CrawlData
    {
        public CrawlData()
        {
            Ports = new List<PortData>();
        }
        public string IPAddress { get; set; }
        public List<PortData> Ports { get;  set; }
        public DateTime ScanDate {  get; set; }

    }
}
