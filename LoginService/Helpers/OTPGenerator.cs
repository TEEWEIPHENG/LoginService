namespace LoginService.Helpers
{
    public class OTPGenerator
    {
        public static string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public static string GenerateReferenceNo()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
