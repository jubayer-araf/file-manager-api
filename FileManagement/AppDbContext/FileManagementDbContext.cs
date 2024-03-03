using FileManagement.Entities;
using Microsoft.EntityFrameworkCore;
namespace FileManagement.AppDbContext
{
    public class FileManagementDbContext : DbContext
    {
        public FileManagementDbContext(DbContextOptions<FileManagementDbContext> options) : base(options)
        {

        }

        public DbSet<FileType> FileTypes { get; set; }
        public DbSet<FolderDetail> FolderDetails { get; set; }
        public DbSet<FileDetail> FileDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<FileType>(b =>
            {
                b.HasKey(r => r.Id);
                b.ToTable("FileTypes");
                b.Property(u => u.Name).HasMaxLength(256);
            });

            builder.Entity<FolderDetail>(b =>
            {
                b.HasKey(r => r.Id);
                b.ToTable("FolderDetails");
            }); 
            
            builder.Entity<FileDetail>(b =>
            {
                b.HasKey(r => r.Id);
                b.ToTable("FileDetails");
            });
        }
    }
}
