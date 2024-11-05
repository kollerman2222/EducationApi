using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FgssrApi.Dtos
{
    public class UserProfileDto
    {
        [Display(Name = "username")]
        public string? Username { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string? PhoneNumber { get; set; }


        [StringLength(250)]
        [Display(Name = "FullNameEnglish")]
        public string? FullNameEnglish { get; set; }


        [StringLength(250)]
        [Display(Name = "FullNameArabic")]
        public string? FullNameArabic { get; set; }


        [StringLength(250)]
        [Display(Name = "Address")]
        public string? Address { get; set; }


        [StringLength(50)]
        public string? Gender { get; set; }


        [DataType(DataType.DateTime)]
        [Display(Name = "Birth date")]
        public DateOnly BirthDate { get; set; }


        public string? ProfileImage { get; set; } = string.Empty;

        [JsonIgnore]
        public IFormFile? EditProfileImage { get; set; }


        [Display(Name = "Current email")]
        public string? CurrentEmail { get; set; }

      
        public string? StudentNumber { get; set; }
        public string? Message { get; set; }
    }
}
