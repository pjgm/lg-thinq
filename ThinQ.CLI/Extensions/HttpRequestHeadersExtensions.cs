using System.Net.Http.Headers;
using ThinQ.CLI.HttpClients;

namespace ThinQ.CLI.Extensions;

public static class HttpRequestHeadersExtensions
{
    public static void SetThinQv2ApiHeadersWithAuth(this HttpRequestHeaders headers, string countryCode, string languageCode, string clientId, string accessToken, string userNo) =>
        headers.AddRange(ApiHeaders.GetAuthorizedThinQv2Headers(countryCode, languageCode, clientId, accessToken, userNo));

    public static void SetThinQv2ApiHeaders(this HttpRequestHeaders headers, string countryCode, string languageCode, string clientId) =>
        headers.AddRange(ApiHeaders.GetThinQv2Headers(countryCode, languageCode, clientId));

    public static void SetEmpApiHeaders(this HttpRequestHeaders headers, string countryCode, string languageCode, string signature, long timestamp) =>
        headers.AddRange(ApiHeaders.GetEmpHeaders(countryCode, languageCode, signature, timestamp));

    public static void SetEmpOAuthApiHeaders(this HttpRequestHeaders headers, string urlEncodedBody, string oauthSecret, string loginSessionId) =>
        headers.AddRange(ApiHeaders.GetEmpOAuthHeaders(urlEncodedBody, oauthSecret, loginSessionId));

    public static void SetAuthorizationServerApiHeaders(this HttpRequestHeaders headers, string relativeUri) =>
        headers.AddRange(ApiHeaders.GetAuthorizationServerHeaders(relativeUri));

    public static void AddRange(this HttpRequestHeaders headers, IEnumerable<(string, string)> values)
    {
        foreach (var (key, value) in values)
        {
            headers.Add(key, value);
        }
    }
}
