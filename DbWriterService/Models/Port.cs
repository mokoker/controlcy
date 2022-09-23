using Common.DTOs.Enums;


namespace DbWriterService.Models
{
    public class Port
    {
        public int Id { get; set; }
        public ushort PortNumber { get; set; }
        public int ScanId { get; set; }
        public Scan Scan { get; set; }
        public PortStatus Status { get; set; }
    }
}
