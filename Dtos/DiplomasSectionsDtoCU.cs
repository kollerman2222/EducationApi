using FgssrApi.Custom_DataAnnotation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FgssrApi.Dtos
{
    public class DiplomasSectionsDtoCU
    {
        [StringLength(200)]
        [Required]
        [Display(Name = "Diploma Name")]
        public string SectionName { get; set; }

        public string SectionImage { get; set; } = string.Empty;

        [StringLength(2500)]
        public string? Description { get; set; }

        public bool isActive { get; set; } = true;

        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [JsonIgnore]
        [MaxFileSize(1)]
        [AllowedFileExtentions(".jpg,.jpeg,.png")]
        public IFormFile? UploadImage { get; set; } = default!;

   
    }
}
