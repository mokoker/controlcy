using Common.DTOs.Enums;

namespace Common.DTOs
{
    public class PortData
    {
        public ushort Port { get; set; }
        public TcpUdp TcpUdp { get; set; }
        public PortStatus Status { get; set; }
    }
}
