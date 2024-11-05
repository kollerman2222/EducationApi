using System.ComponentModel.DataAnnotations.Schema;

namespace FgssrApi.Models
{
    public class SubjectMark
    {
        public int Id { get; set; }

        public string StudentId { get; set; }

        [ForeignKey("StudentId")]

        public ApplicationUser Student { get; set; }


        public int SubjectId { get; set; }

        [ForeignKey("SubjectId")]

        public Subjects Subject { get; set; }

        public string? Grade { get; set; }

        public int? Percentage { get; set; }

        public int Year { get; set; } = DateTime.Now.Year;
    }
}
