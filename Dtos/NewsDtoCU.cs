using System.ComponentModel.DataAnnotations;

namespace FgssrApi.Dtos
{
    public class NewsDtoCU
    {

        [StringLength(250)]
        public string NewsTitle { get; set; } = string.Empty;

        [StringLength(2500)]
        public string NewsDescription { get; set; } = string.Empty;


        public DateOnly? NewsDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}
