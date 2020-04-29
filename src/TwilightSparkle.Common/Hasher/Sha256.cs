using System;
using System.Security.Cryptography;
using System.Text;

namespace TwilightSparkle.Common.Hasher
{
    public class Sha256 : IHasher
    {
        public string GetHash(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return String.Empty;
            }

            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = sha.ComputeHash(bytes);

                return Convert.ToBase64String(hash);
            }
        }
    }
}