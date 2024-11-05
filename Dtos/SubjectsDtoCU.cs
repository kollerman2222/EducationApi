using FgssrApi.Models;
using FgssrApi.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace FgssrApi.Dtos
{
    public class SubjectsDtoCU
    {

        [MaxLength(100)]
        [Required]
        public string SubjectNameEnglish { get; set; } = string.Empty;

        [MaxLength(100)]
        [Required]
        public string SubjectNameArabic { get; set; } = string.Empty;

        public int? SubjectSemester { get; set; }

        [MaxLength(20)]
        public string? SubjectCode { get; set; }

        [MaxLength(100)]
        public string? ScientificDegree { get; set; }

        public int? SubjectHours { get; set; } = 3;

        public int SectionId { get; set; }

       
    }
}
