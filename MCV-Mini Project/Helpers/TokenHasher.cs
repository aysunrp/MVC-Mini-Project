using System.Security.Cryptography;
using System.Text;

namespace MCV_Mini_Project.Helpers
{
    public static class TokenHasher
    {
        public static string Hash(string token)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToHexString(bytes);
        }

        public static bool Matches(string token, string? storedHash)
        {
            if (string.IsNullOrWhiteSpace(storedHash))
                return false;

            var computed = Hash(token);
            if (computed.Length != storedHash.Length)
                return false;

            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(computed),
                Encoding.UTF8.GetBytes(storedHash));
        }
    }
}
