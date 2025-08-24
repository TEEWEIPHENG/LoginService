using System.ComponentModel.DataAnnotations;

namespace LoginService.Data.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; } = Guid.NewGuid().ToString().ToUpper();
        public string Lastname { get; set; } = string.Empty;
        public string Firstname { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string RoleId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public DateTime? LastLogin { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsValid { get; set; } = false;
        public bool IsEnabled { get; set; } = true;
        public bool IsLocked { get; set; } = false;
        public int WrongPasswordCount { get; set; } = 0;
    }
}
