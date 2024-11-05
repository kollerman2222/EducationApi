using System.ComponentModel.DataAnnotations.Schema;

namespace FgssrApi.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } 
        public string JwtAccessId { get; set; } 
        public string UserId { get; set; }       
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
