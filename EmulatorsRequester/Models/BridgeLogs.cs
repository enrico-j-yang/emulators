using System.ComponentModel.DataAnnotations;

namespace EmulatorsRequester.Models
{
    public class BridgeLogs
    {
        [Display(Name = "ID", Description = "Bridge ID")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$")]
        [StringLength(16)]
        [Required]
        public string Id { get; set; }
        [Display(Name = "Count", Description = "Amount of logs")]
        [Range(1, 10000)]
        [Required]
        public int Count { get; set; }
        [Display(Name = "Offset", Description = "Offset of logs")]
        [Range(0, 10000)]
        [Required]
        public int Offset { get; set; }
        public string ResponseStatus { get; set; }
        public string JsonResponse { get; set; }

    }
}
