using System.Security.Cryptography;
using System.Text;

namespace LoginService.Helpers
{
    public class SessionHelper
    {
        public static string GenerateToken(int length = 32)
        {
            var bytes = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes); // URL-safe if you replace +/,= or use Base64Url
        }

        public static string HashToken(string token)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToHexString(bytes); // .NET 5+; else BitConverter
        }
    }
}