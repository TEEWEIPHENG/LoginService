using Microsoft.EntityFrameworkCore;
using LoginService.Data.Repositories;

namespace LoginService.Data
{
    public partial class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public virtual DbSet<UserRepository> Users { get; set; }
        public virtual DbSet<RoleRepository> Roles { get; set; }
        public virtual DbSet<OnboardingLogRepository> OnboardingLogs { get; set; }
        public virtual DbSet<MfaRepository> MFA { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRepository>(entity =>
            {
                entity.HasKey(k => k.Id);
            });
            modelBuilder.Entity<RoleRepository>(entity =>
            {
                entity.HasKey(k => k.Id);
            });
            modelBuilder.Entity<OnboardingLogRepository>(entity =>
            {
                entity.HasKey(k => k.Id);
            });
            modelBuilder.Entity<MfaRepository>(entity =>
            {
                entity.HasKey(k => k.Id);
            });
            modelBuilder.Entity<OnboardingStatusRepository>().HasData(
                new OnboardingStatusRepository { Id = 1, Status = "Success" },
                new OnboardingStatusRepository { Id = 2, Status = "Fail" },
                new OnboardingStatusRepository { Id = 3, Status = "Account Existed" },
                new OnboardingStatusRepository { Id = 4, Status = "Error" }
            );

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
