using ThinQ.CLI.Configuration;
using ThinQ.CLI.Extensions;
using ThinQ.CLI.Models;

namespace ThinQ.CLI.Services;

public class GatewayService
{
    private const string GatewayUriPath = "gateway-uri";
    private readonly HttpClient _httpClient;

    public GatewayService(HttpClient httpClient, UserConfig userConfig)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Domains.GatewayBaseAddress;
        _httpClient.DefaultRequestHeaders
            .SetThinQv2ApiHeaders(userConfig.CountryCode, userConfig.LanguageCode, userConfig.ClientId);
    }

    public async Task<GatewayResponse> GetGateway()
    {
        var response = await _httpClient.GetAsync(GatewayUriPath);

        return await response.ReadContentFromJsonOrThrowAsync<GatewayResponse>();
    }
}
