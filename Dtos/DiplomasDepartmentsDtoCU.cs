using System.ComponentModel.DataAnnotations;

namespace FgssrApi.Dtos
{
    public class DiplomasDepartmentsDtoCU
    {

        [StringLength(200)]
        [Required]
        [Display(Name = "Title")]
        public string DepartmentName { get; set; }

        [StringLength(2500)]
        [Display(Name = "Description")]
        public string? Description { get; set; }
    }
}
