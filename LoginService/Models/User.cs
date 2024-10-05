using System.ComponentModel.DataAnnotations;

namespace LoginService.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }  // Primary key
        [Required]
        [MaxLength(255)]
        public string UserId { get; set; }  // Unique identifier for the user (e.g., GUID)

        [MaxLength(50)]
        public string Lastname { get; set; }  // Nullable

        [MaxLength(50)]
        public string Firstname { get; set; }  // Nullable

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }  // Nullable

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }  // Password is stored as a hashed string

        [Required]
        [MaxLength(255)]
        public string RoleId { get; set; }  // Role of the user, possibly used for role-based access control (e.g., "admin", "basic")

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }  // Email (unique in the system)

        [Required]
        [MaxLength(100)]
        public string MobileNo { get; set; }  // Phone number with a fixed length of 10 characters (nchar(10))

        public DateTime? LastLogin { get; set; }  // Last login date and time, nullable

        [Required]
        public DateTime? CreatedAt { get; set; }  // User creation timestamp, nullable

        [Required]
        public DateTime? UpdatedAt { get; set; }  // Last update timestamp, nullable
    }
}
