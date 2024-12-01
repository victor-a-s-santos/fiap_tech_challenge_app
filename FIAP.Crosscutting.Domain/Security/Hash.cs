using System.Security.Cryptography;
using System.Text;

namespace FIAP.Crosscutting.Domain.Security
{
    public static class Hash
    {
        public static string GenerateHash(string text)
        {
            var hashInBytes = SHA512.Create().ComputeHash(Encoding.ASCII.GetBytes(text));
            return Convert.ToBase64String(hashInBytes, 0, hashInBytes.Length);
        }
    }
}
