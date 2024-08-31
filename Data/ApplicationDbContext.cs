using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ResumeBuilder.Models;

namespace ResumeBuilder.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<PersonalInfo> PersonalInfo { get; set; }        
        public DbSet<ProfileEntry> ProfileEntry { get; set; }
        public DbSet<Resume> Resumes { get; set; } = default!;
    }
}
