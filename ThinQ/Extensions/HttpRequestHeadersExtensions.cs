using System.Net.Http.Headers;

namespace ThinQ.Extensions;

public static class HttpRequestHeadersExtensions
{
    public static void AddRange(this HttpRequestHeaders headers, IEnumerable<(string, string)> values)
    {
        foreach (var (key, value) in values)
        {
            headers.Add(key, value);
        }
    }
}
