using System.ComponentModel.DataAnnotations;

namespace EmulatorsRequester.Models
{
    public class StopSensor
    {
        [Display(Name = "ID", Description = "ID that return by add sensors response")]
        [Required]
        public string Id { get; set; }
        public string ResponseStatus { get; set; }
        public string JsonResponse { get; set; }
    }
}
