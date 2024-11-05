using System.ComponentModel.DataAnnotations;

namespace FgssrApi.Dtos
{
    public class ChangeEmailDto
    {
        [EmailAddress]
        [Display(Name = "New email")]
        public string NewEmail { get; set; }

        public string? VerificationCode { get; set; }
        public string? Message { get; set; }
        public bool isChanged { get; set; }
    }
}
