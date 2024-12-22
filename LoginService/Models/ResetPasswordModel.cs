namespace LoginService.Models
{
    public class ResetPasswordModel
    {
        public string newPassword {  get; set; }
        public string confirmPassword { get; set; }
        public string otp { get; set; }
        public string referenceNo {  get; set; }
    }
}
