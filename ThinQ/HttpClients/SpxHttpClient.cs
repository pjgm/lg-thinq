using ThinQ.Extensions;
using ThinQ.Models;

namespace ThinQ.HttpClients;

internal class SpxHttpClient
{
    private const string PreLoginUriPath = "preLogin";
    private const string SecretOAuthKeyUriPath = "searchKey?key_name=OAUTH_SECRETKEY&sever_type=OP";

    private readonly HttpClient _httpClient;

    public SpxHttpClient(HttpClient httpClient, Uri spxBaseAddress)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = spxBaseAddress;
    }

    public async Task<PreLoginResponse> PreLogin(string username, string password)
    {
        var body = new Dictionary<string, string>
        {
            { "user_auth2", password.ToSha512() },
            { "log_param", $"login request / user_id : {username} / third_party : null / svc_list : SVC202,SVC710 / 3rd_service : " }
        };
        var response = await _httpClient
            .PostAsync(PreLoginUriPath, new FormUrlEncodedContent(body));

        return await response.ReadContentFromJsonOrThrowAsync<PreLoginResponse>();
    }

    public async Task<GetSecretOauthResponse> GetSecretOauthKey()
    {
        var response = await _httpClient.GetAsync(SecretOAuthKeyUriPath);
        return await response.ReadContentFromJsonOrThrowAsync<GetSecretOauthResponse>();
    }
}
