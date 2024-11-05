using System.ComponentModel.DataAnnotations;

namespace FgssrApi.Models
{
    public class DiplomasDepartments
    {
        [Key]
        public int DepartmentId { get; set; }

        [MaxLength(200)]
        [Required]
        public required string DepartmentName { get; set; }

        [MaxLength(2500)]
        public string? Description { get; set; }

        public ICollection<DiplomasSections> Sections { get; set; } = new List<DiplomasSections>();
       
    }
}
