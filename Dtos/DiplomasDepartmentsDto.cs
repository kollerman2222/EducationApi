using FgssrApi.Models;
using System.ComponentModel.DataAnnotations;

namespace FgssrApi.Dtos
{
    public class DiplomasDepartmentsDto
    {
        public int DepId { get; set; }

        [StringLength(200)]
        [Required]
        [Display(Name = "Title")]
        public string DepartmentName { get; set; }

        [StringLength(2500)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        public ICollection<DiplomasSectionsDto> Sections { get; set; } = new List<DiplomasSectionsDto>();
    }
}
