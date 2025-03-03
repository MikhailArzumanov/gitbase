using System.Security.Cryptography;
using System.Text;

namespace Backend.Cryptography {
    public class Hasher {
        private static string GetLSalt(string plain) {
            var l = plain.Length;
            var bytes = BitConverter.GetBytes(l);
            var salt = MD5.HashData(bytes);
            return Convert.ToBase64String(salt);
        }

        private static string GetRSalt(string plain) {
            var l = plain.Length;
            var bytes = BitConverter.GetBytes(l);
            var salt = SHA1.HashData(bytes);
            return Convert.ToBase64String(salt);
        }

        public static string Hash(string plain) {
            string lSalt = GetLSalt(plain), rSalt = GetRSalt(plain);
            var password = Encoding.UTF8.GetBytes(lSalt + plain + rSalt);
            var hash = SHA512.HashData(password);
            return Convert.ToBase64String(hash);
        }
    }
}
