using FgssrApi.Custom_DataAnnotation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FgssrApi.Dtos
{
    public class EventsDtoCU
    {
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

        [JsonIgnore]
        [MaxFileSize(1)]
        [AllowedFileExtentions(".jpg,.jpeg,.png")]
        public IFormFile? UploadImage { get; set; } = default!;

    }
}
