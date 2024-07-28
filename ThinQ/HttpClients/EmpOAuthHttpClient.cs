using ThinQ.Extensions;
using ThinQ.Models;

namespace ThinQ.HttpClients;

internal class EmpOAuthHttpClient
{
    private readonly HttpClient _httpClient;
    private const string EmpOAuthBaseAddress = "https://emp-oauth.lgecloud.com/";
    private const string AuthorizeEmpPath = "emp/oauth2/authorize/empsession";

    public EmpOAuthHttpClient(HttpClient httpClient)
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
