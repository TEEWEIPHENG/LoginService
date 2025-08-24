using Microsoft.EntityFrameworkCore;
using LoginService.Data.Entities;

namespace LoginService.Data
{
    public partial class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<MFA> MFA { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(k => k.Id);
            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(k => k.Id);
            });
            modelBuilder.Entity<MFA>(entity =>
            {
                entity.HasKey(k => k.Id);
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
