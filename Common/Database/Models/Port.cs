using System.ComponentModel.DataAnnotations.Schema;
using Common.DTOs;
using Common.DTOs.Enums;
using Microsoft.EntityFrameworkCore;

namespace DbWriterService.Models
{
    [Table("ports")]
    public class Port
    {
        public int Id { get; set; }

        public ushort PortNumber { get; set; }
        public int ScanId { get; set; }
        public Scan Scan { get; set; }
        public PortStatus Status { get; set; }

        public PortData GetPortData()
        {
            var x= new PortData();
            x.Port = PortNumber;
            x.Status = Status;
            return x;
        }
    }
}
