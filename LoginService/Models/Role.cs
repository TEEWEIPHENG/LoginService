using System.ComponentModel.DataAnnotations;

namespace LoginService.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string RoleId { get; set; } = string.Empty;
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public bool IsEnabled { get; set; } = true;
    }
}
