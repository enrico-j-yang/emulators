using System.ComponentModel.DataAnnotations;

namespace EmulatorsRequester.Models
{
    public class StartTaskModel
    {
        [Display(Name = "MacAddress", Description = "Unique hex(A-F0-9) string 12 characters")]
        [RegularExpression(@"^[a-fA-F0-9\s]*$")]
        [StringLength(12)]
        [Required]
        public string MacAddress { get; set; }
        [Display(Name = "DateTime", Description = "yyyy-MM-ddTHH:mm:ssZ")]
        [Required]
        public string DateTime { get; set; }
        public string ResponseStatus { get; set; }
        public string JsonResponse { get; set; }
    }
}
