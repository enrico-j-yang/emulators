using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace EmulatorsRequester.Models
{
    public class AddSensor
    {
        [Required]
        public string TenantId { get; set; }
        [Required]
        public string AuthId { get; set; }
        [Display(Name = "BearToken", Description = "32 characters token from web protal post request")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$")]
        [StringLength(32, MinimumLength = 32)]
        [Required]
        public string BearToken { get; set; }
        [Display(Name = "BridgePrefix", Description = "Unque string not more than 12 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$")]
        [StringLength(12)]
        [Required]
        public string BridgePrefix { get; set; }
        [Display(Name = "SensorPrefix", Description = "Unque hex(A-F0-9) string not more than 12 characters")]
        [RegularExpression(@"^[a-fA-F0-9\s]*$")]
        [StringLength(12)]
        [Required]
        public string SensorPrefix { get; set; }
        [Display(Name = "SensorCount", Description = "Number that more than 0")]
        [Range(1, 10000)]
        [Required]
        public int SensorCount { get; set; }
        [Display(Name = "MobilePrefix", Description = "Unque string not more than 12 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$")]
        [StringLength(12)]
        [Required]
        public string MobilePrefix { get; set; }
        public string ResponseStatus { get; set; }
        public string JsonResponse { get; set; }
    }

    public static class DisplayAttributeExtension
    {
        public static string GetPropertyDescription<T>(Expression<Func<T>> expression)
        {
            var propertyExpression = (MemberExpression)expression.Body;
            var propertyMember = propertyExpression.Member;
            var displayAttributes = propertyMember.GetCustomAttributes(typeof(DisplayAttribute), true);
            return displayAttributes.Length == 1 ? ((DisplayAttribute)displayAttributes[0]).Description : string.Empty;
        }
    }

}
