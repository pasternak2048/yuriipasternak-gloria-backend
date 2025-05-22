using System.Security.Cryptography;
using System.Text;

namespace GLORIA.BuildingBlocks.Security
{
    public static class SignatureUtils
    {
        public static string ComputeSha512(string input)
        {
            using var sha = SHA512.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
