using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmulatorsRequester.Models
{
    [Table("sensor", Schema = "public")]
    public class Sensor
    {
        [Key]
        public string mac_address { get; set; }
        public string unique_id { get; set; }
        public int battery { get; set; }

    }
}
