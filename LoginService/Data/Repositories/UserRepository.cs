using System.ComponentModel.DataAnnotations;

namespace LoginService.Data.Repositories
{
    public class UserRepository
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string UserId { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Lastname { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Firstname { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string RoleId { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string MobileNo { get; set; } = string.Empty;

        public DateTime? LastLogin { get; set; }

        [Required]
        public DateTime? CreatedAt { get; set; }

        [Required]
        public DateTime? UpdatedAt { get; set; }
        [Required]
        public bool IsValid { get; set; } = false;
        [Required]
        public bool IsEnabled { get; set; } = true;
        [Required]
        public bool IsLocked { get; set; } = false;
        [Required]
        public int WrongPasswordCount { get; set; } = 0;
    }
}
