using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.WebUtilities;

namespace ThinQ.CLI.Models;

public class AuthorizeEmpResponse
{
    public int Status { get; set; } = -1;

    [JsonPropertyName("redirect_uri")]
    public string RedirectUri { get; set; } = string.Empty;

    public string OAuth2Url => RedirectUriParameters.OAuth2BackendUrl;
    public string Code => RedirectUriParameters.Code;

    public RedirectUriParameters RedirectUriParameters
    {
        get
        {
            var queryString = JsonSerializer.Serialize(QueryHelpers.ParseQuery(RedirectUri).ToDictionary(kvp => kvp.Key, kvp => kvp.Value.First()));
            // deserialize the query string into a RedirectUriParameters object using System.Text.Json
            return JsonSerializer.Deserialize<RedirectUriParameters>(queryString);
        }
    }
}

public class RedirectUriParameters
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;
    [JsonPropertyName("state")]
    public string State { get; set; }
    [JsonPropertyName("user_number")]
    public string UserNumber { get; set; } = string.Empty;

    [JsonPropertyName("oauth2_backend_url")]
    public string OAuth2BackendUrl { get; set; } = string.Empty;
}
