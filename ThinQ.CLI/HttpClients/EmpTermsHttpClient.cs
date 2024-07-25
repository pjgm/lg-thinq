using ThinQ.CLI.Extensions;
using ThinQ.CLI.Models;

namespace ThinQ.CLI.HttpClients;

internal class EmpTermsHttpClient
{
    private const string LoginUriPath = "emp/v2.0/account/session";
    private readonly HttpClient _httpClient;

    public EmpTermsHttpClient(HttpClient httpClient, Uri empTermsBaseAddress)
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
