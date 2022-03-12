using System.ComponentModel.DataAnnotations;

namespace EmulatorsRequester.Models
{
    public class Battery
    {
        [Display(Name = "MacAddress", Description = "Unique hex(A-F0-9) string 12 characters")]
        [RegularExpression(@"^[a-fA-F0-9\s]*$")]
        [StringLength(12)]
        [Required]
        public string MacAddress { get; set; }
        [Display(Name = "BatteryLevel", Description = "Percentage 0 to 100")]
        [Range(0, 100)]
        [Required]
        public int BatteryLevel { get; set; }
        public string ResponseStatus { get; set; }
        public string JsonResponse { get; set; }
    }
}
