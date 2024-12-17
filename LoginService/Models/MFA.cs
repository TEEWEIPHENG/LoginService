using System.ComponentModel.DataAnnotations;

namespace LoginService.Models
{
    public class MFA
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string OTP { get; set; } = string.Empty;
        [Required]
        public string ReferenceNo { get; set; } = string.Empty;
        [Required]
        public string UserId{ get; set; } = string.Empty;
        [Required]
        public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}
