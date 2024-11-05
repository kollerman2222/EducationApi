using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FgssrApi.Models
{
    public class ApplicationUser:IdentityUser
    {
        [MaxLength(250)]
       public string FullNameEnglish { get; set; } = string.Empty;
        [MaxLength(250)]
        public string FullNameArabic { get; set; } = string.Empty;

        [MaxLength (20)]
        public string? EmailVerifyCode { get; set; }

        [MaxLength(250)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? PhoneVerifyCode { get; set; }

        public DateTime PhoneCodeCreated { get; set; }

        public DateTime EmailCodeCreated { get; set; }

        public bool PhoneConfirmed { get; set; } = false;

        [MaxLength(200)]
        public string? ProfileImage { get; set; }

        [MaxLength(200)]
        public string? ApplicationStatus { get; set; }

        [MaxLength(100)]
        public string GraduationCertUpload { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Gender { get; set; } = string.Empty;

        public DateOnly BirthDate { get; set; }

        public int? SectionId { get; set; }

        public DiplomasSections? Section { get; set; }


        public string? DeclineReason { get; set; }

        public string? AdmissionNumber { get; set; }

        public string? StudentNumber { get; set; }


        public ICollection<ChatMessages>? SentMessages { get; set; }

        public ICollection<SubjectMark>? SubjectMark { get; set; }

    }
}
