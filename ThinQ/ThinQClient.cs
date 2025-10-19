using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using ThinQ.Extensions;
using ThinQ.Models;
using ThinQ.SessionManagement;

namespace ThinQ;

public class ThinQClient
{
    private const string DevicesPath = "devices";
    private const string DeviceProfilePath = "devices/{0}/profile";
    private const string DeviceStatusPath = "devices/{0}/state";
    private const string DeviceControlPath = "devices/{0}/control";
    
    private const string RegisterIoTCertificatePath = "service/users/client/certificate";

    private readonly HttpClient _httpClient;
    
    private readonly JsonSerializerOptions _deviceControlJsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new JsonStringEnumConverter() }
    };

    public ThinQClient(Session session) : this(session.ThinqApiUri, session.CountryCode, session.PersonalAccessToken) { }

    public ThinQClient(Uri baseUri, string country, string apiKey)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = baseUri
        };
        
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        _httpClient.DefaultRequestHeaders.Add("x-api-key", "v6GFvkweNo7DK7yD3ylIZ9w52aKBU0eJ7wLXkSR3");
        _httpClient.DefaultRequestHeaders.Add("x-country", country);
        _httpClient.DefaultRequestHeaders.Add("x-message-id", "fNvdZ1brTn-wWKUlWGoSVw");
        _httpClient.DefaultRequestHeaders.Add("x-client-id", "web");
        _httpClient.DefaultRequestHeaders.Add("x-service-phase", "OP");
    }

    public async Task<GetDevicesResponse> GetDevices()
    {
        var response = await _httpClient.GetAsync(DevicesPath);

        return await response.ReadContentFromJsonOrThrowAsync<GetDevicesResponse>();
    }
    
    public async Task<DeviceProfile> GetDeviceProfile(string deviceId)
    {
        var response = await _httpClient.GetAsync(string.Format(DeviceProfilePath, deviceId));
        
        var deviceCapabilities = await response.ReadCapabilitiesFromJsonOrThrowAsync();

        return await response.ReadContentFromJsonOrThrowAsync<DeviceProfile>();
    }
    
    public async Task<DeviceStateResponse> GetDeviceState(string deviceId)
    {
        var response = await _httpClient.GetAsync(string.Format(DeviceStatusPath, deviceId));

        return await response.ReadContentFromJsonOrThrowAsync<DeviceStateResponse>();
    }
    
    public async Task TurnOnAc(string deviceId)
    {
        var test = new DeviceState
        {
            Operation = new OperationInfo(AirConOperationMode.POWER_OFF)
        };
        
        var serialized = JsonSerializer.Serialize(test, _deviceControlJsonSerializerOptions);

        var content = new StringContent(serialized, null, MediaTypeNames.Application.Json);

        var response = await _httpClient.PostAsync(string.Format(DeviceControlPath, deviceId), content);

        return;
    }
    
    
    public async Task<RegisterIoTCertificateResponse> RegisterIotCertificate(string csr)
    {
        var httpContent = new StringContent(JsonSerializer.Serialize(
            new Dictionary<string, string>
            {
                { "csr", csr },
            }), null, MediaTypeNames.Application.Json);

        var response = await _httpClient.PostAsync(RegisterIoTCertificatePath, httpContent);

        return await response.ReadContentFromJsonOrThrowAsync<RegisterIoTCertificateResponse>();
    }
}