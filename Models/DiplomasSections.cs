using System.ComponentModel.DataAnnotations;

namespace FgssrApi.Models
{
    public class DiplomasSections
    {
        [Key]
        public int SectionId { get; set; }

        [MaxLength(200)]
        [Required]
        public required string SectionName { get; set; }

        [MaxLength (2500)]
        public string? Description { get; set; }

        public bool isActive { get; set; } = true;

        [MaxLength(1000)]
        [Required]
        public string SectionImage { get; set; } = string.Empty;

        public int DepartmentId { get; set; }

        public  DiplomasDepartments? Department { get; set; }

        public ICollection<Subjects> Subjects { get; set; } = new List<Subjects>();

        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();



    }
}
