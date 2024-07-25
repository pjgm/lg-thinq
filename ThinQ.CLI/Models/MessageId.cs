using System.Text.RegularExpressions;

namespace ThinQ.CLI.Models;

public static class MessageId
{
    public static string Generate()
    {
        var guid = Guid.NewGuid();
        var base64Guid = Convert.ToBase64String(guid.ToByteArray());
        return new Regex("=*$").Replace(base64Guid, "");
    }
}
