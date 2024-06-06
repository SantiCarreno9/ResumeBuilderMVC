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

        public DbSet<User> Users { get; set; }
        public DbSet<UserInfo> UsersInfo { get; set; }
        public DbSet<ProfessionalInfo> ProfessionalInfo { get; set; }
        public DbSet<ProfileEntry> ProfileEntry { get; set; }
    }
}
