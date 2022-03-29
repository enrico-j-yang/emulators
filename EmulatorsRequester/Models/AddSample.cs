using System.ComponentModel.DataAnnotations;

namespace EmulatorsRequester.Models
{
    public class AddSample
    {
        [Display(Name = "MacAddress", Description = "Unique hex(A-F0-9) string 12 characters")]
        [RegularExpression(@"^[a-fA-F0-9\s]*$")]
        [StringLength(12)]
        public string MacAddress { get; set; }
        [Display(Name = "Samples", Description = "comma separated values")]
        public string Sample { get; set; }
        [Display(Name = "RandomRangeLow", Description = "Random samples range begin value")]
        public int RandomRangeLow { get; set; }
        [Display(Name = "RandomRangeHigh", Description = "Random samples range end value")]
        public int RandomRangeHigh { get; set; }
        [Display(Name = "RandomAmount", Description = "Random samples count")]
        public int RandomAmount { get; set; }
        public string ResponseStatus { get; set; }
        public string JsonResponse { get; set; }
    }
}
