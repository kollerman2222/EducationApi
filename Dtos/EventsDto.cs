using FgssrApi.Custom_DataAnnotation;
using System.ComponentModel.DataAnnotations;

namespace FgssrApi.Dtos
{
    public class EventsDto
    {
        public int EveId { get; set; }
        public required string EventTitle { get; set; }

        public string? EventImageName { get; set; } = string.Empty;

        [StringLength(2500)]
        public string EventDescription { get; set; } = string.Empty;

        [StringLength(1500)]
        public string EventLocation { get; set; } = string.Empty;

        public int Time { get; set; }

        public int DateDay { get; set; }

        [StringLength(150)]
        public string DateMonth { get; set; } = string.Empty;
       
    }
}
