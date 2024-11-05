using System.ComponentModel.DataAnnotations;

namespace FgssrApi.Models
{
    public class Staff
    {
        [Key]
        public int StaffId { get; set; }
        public string StaffName { get; set; } = string.Empty;

        public string StaffPosition { get; set; } = string.Empty;

        [MaxLength(1000)]
        [Required]
        public string StaffImage { get; set; } = string.Empty;

        [MaxLength(2500)]
        public string Biograpghy { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Email { get; set; } = string.Empty;
    }
}
