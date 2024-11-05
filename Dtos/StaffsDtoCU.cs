using FgssrApi.Custom_DataAnnotation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FgssrApi.Dtos
{
    public class StaffsDtoCU
    {

        [Display(Name = "Staff Name")]
        public string StaffName { get; set; } = string.Empty;

        [Display(Name = "Position")]

        public string StaffPosition { get; set; } = string.Empty;

        [JsonIgnore]
        [MaxFileSize(1)]
        [AllowedFileExtentions(".jpg,.jpeg,.png")]
        public IFormFile? UploadImage { get; set; } = default!;

        [StringLength(2500)]
        [Display(Name = "Biograpghy")]
        public string Biograpghy { get; set; } = string.Empty;


        [StringLength(1000)]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
    }
}
