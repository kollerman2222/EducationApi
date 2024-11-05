using System.ComponentModel.DataAnnotations;

namespace FgssrApi.Models
{
    public class Subjects
    {
        [Key]
        public int SubjectId { get; set; }

        [MaxLength(100)]
        public string SubjectNameEnglish { get; set; } = string.Empty;

        [MaxLength(100)]
        public string SubjectNameArabic { get; set; } = string.Empty;


        [MaxLength(20)]
        public string? SubjectCode { get; set; }

        [MaxLength(100)]
        public string? ScientificDegree { get; set; }


        public int? SubjectHours { get; set; }

        public int? SubjectSemester { get; set; }

        public int SectionId { get; set; }

        public DiplomasSections? Section { get; set; }


        public ICollection<SubjectMark>? SubjectMark { get; set; }


    }
}
