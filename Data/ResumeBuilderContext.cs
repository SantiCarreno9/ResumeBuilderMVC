using Microsoft.EntityFrameworkCore;
using ResumeBuilder.Models;

namespace ResumeBuilder.Data
{
    public class ResumeBuilderContext : DbContext
    {
        public ResumeBuilderContext (DbContextOptions<ResumeBuilderContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<PersonalInfo> PersonalInfo { get; set; }
        public DbSet<ProfileInfo> ProfileInfo { get; set; }
        public DbSet<ProfileEntry> ProfileEntry { get; set; }
        public DbSet<Resume> Resumes { get; set; } = default!;
    }
}
