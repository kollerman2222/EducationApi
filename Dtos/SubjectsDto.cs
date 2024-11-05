using FgssrApi.Models;
using FgssrApi.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace FgssrApi.Dtos
{
    public class SubjectsDto
    {
        public int SubjectId { get; set; }

        [MaxLength(100)]
        [Required]
        public string SubjectNameEnglish { get; set; } = string.Empty;

        [MaxLength(100)]
        [Required]
        public string SubjectNameArabic { get; set; } = string.Empty;


        [MaxLength(20)]
        public string? SubjectCode { get; set; }

        [MaxLength(100)]
        public string? ScientificDegree { get; set; }

        public int? SubjectHours { get; set; } = 3;
        public int? SubjectSemester { get; set; }

        public string? SectionName { get; set; }
        public string? DepartmentName { get; set; }


  
    }
}
