using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FgssrApi.Models
{
    public class ChatMessages
    {

        public int Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public string? SenderId { get; set; }

        [ForeignKey("SenderId")]
        public ApplicationUser? Sender { get; set; }

        public string? ProfileImage { get; set; }
        public string? SenderName { get; set; } = string.Empty;

    }
}
