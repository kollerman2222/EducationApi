using FgssrApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FgssrApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }

        public DbSet<DiplomasDepartments> DiplomasDepartments { get; set; }

        public DbSet<DiplomasSections> DiplomasSections { get; set; }

        public DbSet<Events> Events { get; set; }

        public DbSet<News> News { get; set; }

        public DbSet<Staff> Staff { get; set; }

        public DbSet<Subjects> Subjects { get; set; }

        public DbSet<PrivateMessage> PrivateMessage { get; set; }

        public DbSet<ChatMessages> ChatMessages { get; set; }

        public DbSet<SubjectMark> SubjectMark { get; set; }

        public DbSet<RefreshToken> RefreshToken { get; set; }

    }
}
