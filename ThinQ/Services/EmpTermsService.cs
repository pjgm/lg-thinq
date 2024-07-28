using System.Net.Http.Headers;
using ThinQ.Extensions;
using ThinQ.Models;

namespace ThinQ.Services;

internal class EmpTermsService
{
    private const string LoginUriPath = "emp/v2.0/account/session";
    private readonly HttpClient _httpClient;

    public EmpTermsService(HttpClient httpClient, Uri empTermsBaseAddress)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = empTermsBaseAddress;
    }

    public async Task<LoginResponse> Login(
        string countryCode,
        string languageCode,
        string username,
        string signature,
        long timestamp,
        string encryptedPassword)
    {
        var body = new Dictionary<string, string>
        {
            { "user_auth2", encryptedPassword },
            { "password_hash_prameter_flag", "Y" },
            { "svc_list", "SVC202,SVC710" },
            { "inactive_policy", "N" }
        };

        var httpContent = new FormUrlEncodedContent(body);
        var escapedUserName = Uri.EscapeDataString(username);
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{LoginUriPath}/{escapedUserName}")
        {
            Content = httpContent
        };

        requestMessage.Headers.SetEmpApiHeaders(countryCode, languageCode, signature, timestamp);

        var response = await _httpClient.SendAsync(requestMessage);

        return await response.ReadContentFromJsonOrThrowAsync<LoginResponse>();
    }
}

public static class EmpTermsHttpRequestHeadersExtensions
{
    public static void SetEmpApiHeaders(this HttpRequestHeaders headers, string countryCode, string languageCode, string signature, long timestamp) =>
        headers.AddRange(GetEmpHeaders(countryCode, languageCode, signature, timestamp));
    private static IEnumerable<(string, string)> GetEmpHeaders(string countryCode, string languageCode, string signature, long timestamp) =>
        new List<(string, string)>
        {
            ( "Accept", "application/json" ),
            ( "X-Application-Key", "6V1V8H2BN5P9ZQGOI5DAQ92YZBDO3EK9" ),
            ( "X-Client-App-Key", "LGAO221A02" ),
            ( "X-Lge-Svccode", "SVC709" ),
            ( "X-Device-Type", "M01" ),
            ( "X-Device-Platform", "ADR" ),
            ( "X-Device-Language-Type", "IETF" ),
            ( "X-Device-Publish-Flag", "Y" ),
            ( "X-Device-Country", countryCode ),
            ( "X-Device-Language", languageCode ),
            ( "Access-Control-Allow-Origin", "*" ),
            ( "Accept-Encoding", "gzip, deflate, br" ),
            ( "Accept-Language", "en-US,en;q=0.9" ),
            ( "X-Signature", signature ),
            ( "X-Timestamp", timestamp.ToString() )
        };


}
