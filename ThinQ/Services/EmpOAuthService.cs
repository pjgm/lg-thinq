using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using ThinQ.Extensions;
using ThinQ.Models;

namespace ThinQ.Services;

internal class EmpOAuthService
{
    private readonly HttpClient _httpClient;
    private const string EmpOAuthBaseAddress = "https://emp-oauth.lgecloud.com/";
    private const string AuthorizeEmpPath = "emp/oauth2/authorize/empsession";

    public EmpOAuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(EmpOAuthBaseAddress);
    }

    public async Task<AuthorizeEmpResponse> AuthorizeEmp(
        string userId,
        string userIdType,
        string country,
        string oauthSecret,
        string loginSessionId)
    {

        var body = new Dictionary<string, string>
        {
            { "account_type", userIdType },
            { "client_id", "LGAO221A02" },
            { "country_code", country },
            { "redirect_uri", "lgaccount.lgsmartthinq:/" },
            { "response_type", "code" },
            { "state", "12345" },
            { "username", userId }
        };

        var urlEncodedBody = new FormUrlEncodedContent(body);
        var urlEncodedBodyString = await urlEncodedBody.ReadAsStringAsync();


        var requestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            $"{AuthorizeEmpPath}?{urlEncodedBodyString}");

        requestMessage.Headers.SetEmpOAuthApiHeaders(urlEncodedBodyString, oauthSecret, loginSessionId);

        var response = await _httpClient.SendAsync(requestMessage);

        return await response.ReadContentFromJsonOrThrowAsync<AuthorizeEmpResponse>();
    }
}


public static class EmpOAuthHttpRequestHeadersExtensions
{
    public static void SetEmpOAuthApiHeaders(this HttpRequestHeaders headers, string urlEncodedBody, string oauthSecret, string loginSessionId) =>
        headers.AddRange(GetEmpOAuthHeaders(urlEncodedBody, oauthSecret, loginSessionId));
    private static IEnumerable<(string, string)> GetEmpOAuthHeaders(string urlEncodedBody, string oauthSecret, string loginSessionId)
    {
        var relativeUrlEncoded = $"/emp/oauth2/authorize/empsession?{urlEncodedBody}";

        var now = DateTime.UtcNow;
        var timestamp = now.ToString("ddd',' d MMM yyyy HH':'mm':'ss", CultureInfo.InvariantCulture) +
                        " " + now.ToString("zzzz").Replace(":", "");
        var secret = Encoding.UTF8.GetBytes(oauthSecret);
        var messageString = $"{relativeUrlEncoded}\n{timestamp}";
        var message = Encoding.UTF8.GetBytes(messageString);
        var hash = new HMACSHA1(secret).ComputeHash(message);
        var signature = Convert.ToBase64String(hash);
        return new List<(string, string)>
        {
            ( "lgemp-x-app-key", "LGAO722A02" ),
            ( "lgemp-x-date", timestamp ),
            ( "lgemp-x-session-key", loginSessionId ),
            ( "lgemp-x-signature", signature ),
            ( "X-Device-Type", "M01" ),
            ( "X-Device-Platform", "ADR" ),
            ( "User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.63 Safari/537.36 Edg/93.0.961.44" ),
            ( "Access-Control-Allow-Origin", "*" ),
            ( "Accept-Encoding", "gzip, deflate, br" ),
            ( "Accept-Language", "en-US,en;q=0.9" )
        };
    }
}
