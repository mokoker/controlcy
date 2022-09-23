using System.ComponentModel.DataAnnotations;

namespace DbWriterService.Models
{
    public class Address
    {
        [Key]
        public string Ip { get; set; }
        public List<Scan> Scans { get; set; }

    }
}
