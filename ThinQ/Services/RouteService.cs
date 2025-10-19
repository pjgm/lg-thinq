using ThinQ.Extensions;
using ThinQ.Models;

namespace ThinQ.Services;

public class RouteService
{
    private const string RoutePath = "route";
    
    // Europe, Middle East, Africa
    private readonly Uri _baseUri = new("https://api-eic.lgthinq.com");
    private readonly HttpClient _httpClient;

    public RouteService(string apiToken, string countryCode)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = _baseUri
        };
        
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiToken}");
        _httpClient.DefaultRequestHeaders.Add("x-api-key", "v6GFvkweNo7DK7yD3ylIZ9w52aKBU0eJ7wLXkSR3");
        _httpClient.DefaultRequestHeaders.Add("x-country", countryCode);
        _httpClient.DefaultRequestHeaders.Add("x-message-id", "fNvdZ1brTn-wWKUlWGoSVw");
        _httpClient.DefaultRequestHeaders.Add("x-client-id", "web");
        _httpClient.DefaultRequestHeaders.Add("x-service-phase", "OP");
    }
    
    // An API to get the Backend address for the ThinQ Platform.
    // Views the domain name by region and configuration.
    public async Task<RouteResponse> GetRoute()
    {
        
        var response = await _httpClient.GetAsync(RoutePath);
        
        return await response.ReadContentFromJsonOrThrowAsync<RouteResponse>();
    }
}