using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbWriterService.Models
{
    [Table("addresses")]
    public class Address
    {
        [Key]
        [Column(TypeName= "varchar(15)")]
        public string Ip { get; set; }
        public List<Scan> Scans { get; set; }

    }
}
