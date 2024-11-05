using FgssrApi.Custom_DataAnnotation;
using FgssrApi.Models;
using System.ComponentModel.DataAnnotations;

namespace FgssrApi.Dtos
{
    public class AdmissionDto
    {
        [Required]
        public string userId { get; set; }

        [Required]
        public int sectionId { get; set; }

        [Required]
        [MaxFileSize(1)]
        [AllowedFileExtentions(".jpg,.jpeg,.png,.pdf")]
        public IFormFile GraduationCert { get; set; }


    }
}
