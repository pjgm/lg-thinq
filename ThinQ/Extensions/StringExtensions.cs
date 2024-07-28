using System.Security.Cryptography;
using System.Text;

namespace ThinQ.Extensions;

public static class StringExtensions
{
    public static string ToSha512(this string input)
    {
        var data = Encoding.UTF8.GetBytes(input);
        var result = SHA512.HashData(data);
        return Convert.ToHexString(result).ToLower();
    }
}
