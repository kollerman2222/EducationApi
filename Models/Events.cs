using System.ComponentModel.DataAnnotations;

namespace FgssrApi.Models
{
    public class Events
    {
        [Key]
        public int EventId { get; set; }

        [MaxLength(250)]
        [Required]
        public required string EventTitle { get; set; }

        [MaxLength(2500)]
        public string EventDescription { get; set; } = string.Empty;

        [MaxLength(1500)]
        public string EventLocation { get; set; } = string.Empty;

        public int Time { get; set; }

        public int DateDay { get; set; }

        [MaxLength(150)]
        public string DateMonth { get; set; } = string.Empty;

        [MaxLength(1000)]
        [Required]
        public string EventImage { get; set; } = string.Empty;

        
    }
}
