namespace DbWriterService.Models
{
    public class Scan
    {
        public int Id { get; set; }
        public DateTime ScanDate { get; set; }
        public Address Address { get; set; }
        public List<Port> Ports { get; set; }
        public string AddressIp { get; set; }
    }
}
