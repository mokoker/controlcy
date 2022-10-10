using System.Text.Json.Serialization;
using Common.DTOs.Enums;

namespace Common.DTOs
{
    public class PortData
    {
        public ushort Port { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]

        public TcpUdp TcpUdp { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]

        public PortStatus Status { get; set; }
    }
}
