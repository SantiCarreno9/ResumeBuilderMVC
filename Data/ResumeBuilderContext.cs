using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public DbSet<ResumeBuilder.Models.UserInfo> UserInfo { get; set; } = default!;
    }
}
