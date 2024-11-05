using System.ComponentModel.DataAnnotations;

namespace FgssrApi.Dtos
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "FullNameEnglish")]
        public string FullNameEnglish { get; set; }

        [Phone]
        [Required]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }


        [Required]
        [StringLength(250)]
        [Display(Name = "FullNameArabic")]
        public string FullNameArabic { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string Gender { get; set; } = string.Empty;

        //[Required]
        //[DataType(DataType.DateTime)]
        //public DateOnly BirthDate { get; set; }
    }
}
