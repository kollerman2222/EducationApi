using System.ComponentModel.DataAnnotations;

namespace FgssrApi.Dtos
{
    public class ChangePhoneNumberDto
    {
       
        [Phone]
        [Display(Name = "New PhoneNumber")]
        public string NewPhoneNumber { get; set; }

        public string? Message { get; set; }

        public bool isChanged { get; set; }
    }
}
