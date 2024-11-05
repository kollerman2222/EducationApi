using System.ComponentModel.DataAnnotations;

namespace FgssrApi.Models
{

    public class News
    {
        [Key]
        public int NewsId { get; set; }

        [MaxLength(250)]
        public string NewsTitle { get; set; } = string.Empty;

        [MaxLength(2500)]
        public string NewsDescription { get; set;} = string.Empty;

        public DateOnly? NewsDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    }
}
