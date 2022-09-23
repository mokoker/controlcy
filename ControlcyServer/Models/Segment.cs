using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlcyServer.Models
{
    [Table("Segment")]
    public class Segment
    {
        [Key]
        public int Id { get; set; }
        public string CIDR { get; set; }
        public DateTime LastScan { get; set; }
    }
}
