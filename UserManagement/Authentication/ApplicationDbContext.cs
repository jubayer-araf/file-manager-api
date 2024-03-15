using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using UserManagement.Entities;

namespace UserManagement.Authentication
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<UserSetting> UserSettings { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserGroupMapping> UserGroupMappings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<UserSetting>(b =>
            {
                b.HasKey(r => r.Id);
                b.ToTable("UserSettings");
            });

            builder.Entity<UserGroup>(b =>
            {
                b.HasKey(r => r.Id);
                b.ToTable("UserGroups");
                b.Property(u => u.Name).HasMaxLength(256);
            });

            builder.Entity<UserGroupMapping>(b =>
            {
                b.HasKey(r => r.Id);
                b.ToTable("UserGroupMapping");
            });
        }
    }
}
