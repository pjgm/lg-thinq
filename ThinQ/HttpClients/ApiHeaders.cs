using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using ThinQ.Models;

namespace ThinQ.HttpClients;

public static class ApiHeaders
{
    public static IEnumerable<(string, string)> GetAuthorizedThinQv2Headers(
        string countryCode,
        string languageCode,
        string clientId,
        string accessToken,
        string userNo)
    {
        var apiHeaders = GetThinQv2Headers(countryCode, languageCode, clientId).ToList();
        apiHeaders.Add(("Authorization", $"Bearer {accessToken}"));
        apiHeaders.Add(("x-emp-token", accessToken));
        apiHeaders.Add(("x-user-no", userNo));
        apiHeaders.Add(("country_code", countryCode));
        apiHeaders.Add(("language_code", languageCode));
        return apiHeaders;
    }

    public static IEnumerable<(string, string)> GetThinQv2Headers(string countryCode, string languageCode, string clientId)
        => new List<(string, string)>
        {
            ("client_id", clientId),
            ("x-client-id", clientId),
            ("x-message-id", MessageId.Generate()),
            ("x-api-key", "VGhpblEyLjAgU0VSVklDRQ=="),
            ("x-service-code", "SVC202"),
            ("x-service-phase", "OP"),
            ("x-thinq-app-level", "PRD"),
            ("x-thinq-app-os", "ANDROID"),
            ("x-thinq-app-type", "NUTS"),
            ("x-thinq-app-ver", "3.0.1700"),
            ("x-country-code", countryCode),
            ("x-language-code", languageCode)
        };

    public static IEnumerable<(string, string)> GetEmpHeaders(string countryCode, string languageCode, string signature, long timestamp) =>
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

    public static IEnumerable<(string, string)> GetEmpOAuthHeaders(string urlEncodedBody, string oauthSecret, string loginSessionId)
    {
        string relativeUrlEncoded = $"/emp/oauth2/authorize/empsession?{urlEncodedBody}";

        var now = DateTime.UtcNow;
        string timestamp = now.ToString("ddd',' d MMM yyyy HH':'mm':'ss", CultureInfo.InvariantCulture) +
                           " " + now.ToString("zzzz").Replace(":", "");
        byte[] secret = Encoding.UTF8.GetBytes(oauthSecret);
        string messageString = $"{relativeUrlEncoded}\n{timestamp}";
        byte[] message = Encoding.UTF8.GetBytes(messageString);
        byte[] hash = new HMACSHA1(secret).ComputeHash(message);
        string signature = Convert.ToBase64String(hash);
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

    public static IEnumerable<(string, string)> GetAuthorizationServerHeaders(string relativeUri)
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
