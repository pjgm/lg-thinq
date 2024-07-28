using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using ThinQ.Extensions;
using ThinQ.Models;

namespace ThinQ.Services;

// Enterprise Membership Platform
// https://thinq.developer.lge.com/en/cloud/docs/EMP-Authorization/overview/
internal class AuthorizationServerService
{
    private const string OAuthTokenUriPath = "oauth/1.0/oauth2/token";
    private const string UserProfileUriPath = "oauth/1.0/users/profile";

    private readonly HttpClient _httpClient;

    public AuthorizationServerService(HttpClient httpClient, Uri authorizationServerBaseAddress)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = authorizationServerBaseAddress;
    }

    public async Task<Oauth2Response> GetOAuthToken(string code)
    {
        var queryParams = new Dictionary<string, string>
        {
            { "code", code },
            { "grant_type", "authorization_code" },
            { "redirect_uri", "lgaccount.lgsmartthinq:/" }
        };
        HttpContent httpContent = new FormUrlEncodedContent(queryParams);

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, OAuthTokenUriPath)
        {
            Content = httpContent
        };

        var relativeUrlEncoded = $"/{OAuthTokenUriPath}?{await requestMessage.Content.ReadAsStringAsync()}";

        requestMessage.Headers.SetAuthorizationServerApiHeaders(relativeUrlEncoded);

        var response = await _httpClient.SendAsync(requestMessage);

        return await response.ReadContentFromJsonOrThrowAsync<Oauth2Response>();
    }

    public async Task<Oauth2Response> RefreshToken(string refreshToken)
    {
        var queryParams = new Dictionary<string, string>
        {
            { "grant_type", "refresh_token" },
            { "refresh_token", refreshToken }
        };

        HttpContent httpContent = new FormUrlEncodedContent(queryParams);

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, OAuthTokenUriPath)
        {
            Content = httpContent
        };

        var relativeUrlEncoded = $"/{OAuthTokenUriPath}?{await requestMessage.Content.ReadAsStringAsync()}";

        requestMessage.Headers.SetAuthorizationServerApiHeaders(relativeUrlEncoded);

        var response = await _httpClient.SendAsync(requestMessage);

        return await response.ReadContentFromJsonOrThrowAsync<Oauth2Response>();
    }

    public async Task<UserProfileResponse> GetUserProfile(string accessToken)
    {

        var queryParams = new Dictionary<string, string>
        {
            { "access_code", accessToken },
        };
        HttpContent httpContent = new FormUrlEncodedContent(queryParams);

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, UserProfileUriPath)
        {
            Content = httpContent
        };

        requestMessage.Headers.SetAuthorizationServerApiHeaders($"/{UserProfileUriPath}");
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(requestMessage);

        return await response.ReadContentFromJsonOrThrowAsync<UserProfileResponse>();

    }
}

public static class AuthorizationServerHttpRequestHeadersExtensions
{
    public static void SetAuthorizationServerApiHeaders(this HttpRequestHeaders headers, string relativeUri) =>
        headers.AddRange(GetAuthorizationServerHeaders(relativeUri));
    private static IEnumerable<(string, string)> GetAuthorizationServerHeaders(string relativeUri)
    {
        var now = DateTime.UtcNow;
        var timestamp = now.ToString("ddd',' d MMM yyyy HH':'mm':'ss", CultureInfo.InvariantCulture) +
                        " " + now.ToString("zzzz").Replace(":", "");
        var secret = Encoding.UTF8.GetBytes("c053c2a6ddeb7ad97cb0eed0dcb31cf8");
        var messageString = $"{relativeUri}\n{timestamp}";
        var message = Encoding.UTF8.GetBytes(messageString);
        var hash = new HMACSHA1(secret).ComputeHash(message);
        var signature = Convert.ToBase64String(hash);

        return new List<(string, string)>
        {
            ( "Accept", "application/json; charset=UTF-8" ),
            ( "x-lge-oauth-signature", signature ),
            ( "x-lge-oauth-date", timestamp ),
            ( "x-lge-appkey", "LGAO221A02" )
        };
    }
}
