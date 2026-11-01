using System.Security.Cryptography;
using System.Text;

namespace PAP.Business.Core
{
    /// <summary>
    /// Turns a plain password into the value stored in the PasswordHash columns (SHA-256, hexadecimal).
    /// </summary>
    public static class PasswordHasher
    {
        private const int HashLength = 64;

        public static string Hash(string plainPassword)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(plainPassword));
                var builder = new StringBuilder(HashLength);
                foreach (var value in bytes)
                {
                    builder.Append(value.ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
