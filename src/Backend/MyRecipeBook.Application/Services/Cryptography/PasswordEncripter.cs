using System.Security.Cryptography;
using System.Text;

namespace MyRecipeBook.Application.Services.Cryptography
{
    public class PasswordEncripter
    {
        public string Encrypt(string password)
        {
            var key = "ABC";

            var newPassword = $"{password}{key}";

            var bytes = Encoding.UTF8.GetBytes(newPassword);

            var hashBytes = SHA512.HashData(bytes);

            return StringBytes(hashBytes);
        }

        private static string StringBytes(byte[] bytes)
        {
            var stringBuilder = new StringBuilder();

            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                stringBuilder.Append(hex);
            }
            return stringBuilder.ToString();
        }
    }
}
