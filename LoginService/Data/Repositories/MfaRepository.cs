using System.ComponentModel.DataAnnotations;

namespace LoginService.Data.Repositories
{
    public class MfaRepository
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string OTP { get; set; } = string.Empty;
        [Required]
        public string ReferenceNo { get; set; } = string.Empty;
        [Required]
        public string UserId { get; set; } = string.Empty;
        [Required]
        public int IsValid { get; set; } = 0;
        [Required]
        public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}
