using FgssrApi.Custom_DataAnnotation;
using FgssrApi.Models;
using FgssrApi.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace FgssrApi.Dtos
{
    public class UsersInfoDto
    {
        public string UserId { get; set; }

        public string FullNameEnglish { get; set; } = string.Empty;

        public string FullNameArabic { get; set; } = string.Empty;

        public string? Email { get; set; }
        public DateOnly BirthDate { get; set; }

        public string? PhoneNumber { get; set; }

        public string? ApplicationStatus { get; set; }

        public string? ProfileImage { get; set; }

        public int? SectionId { get; set; }

        public string? SectionName { get; set; }


        public List<RoleDto> Roles { get; set; } = new List<RoleDto>();

   
    }
}
