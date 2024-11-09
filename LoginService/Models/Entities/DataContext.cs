using Microsoft.EntityFrameworkCore;
using LoginService.Models;

namespace LoginService.Models.Entities
{
    public partial class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<OnboardingLog> OnboardingLogs{ get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity => {
                entity.HasKey(k => k.Id);
            });
            modelBuilder.Entity<Role>(entity => {
                entity.HasKey(k => k.Id);
            });
            modelBuilder.Entity<OnboardingLog>(entity => {
                entity.HasKey(k => k.Id);
            });

            // Seed data for OnboardingStatus
            modelBuilder.Entity<OnboardingStatus>().HasData(
                new OnboardingStatus { Id = 1, Status = "Success" },
                new OnboardingStatus { Id = 2, Status = "Fail" },
                new OnboardingStatus { Id = 3, Status = "Account Existed" },
                new OnboardingStatus { Id = 4, Status = "Error" }
            );

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
