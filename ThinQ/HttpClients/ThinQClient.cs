using System.Text.Json;
using ThinQ.Extensions;
using ThinQ.Models;
using ThinQ.SessionManagement;

namespace ThinQ.HttpClients;

public class ThinQClient
{
    private const string RouteUri = "https://common.lgthinq.com/route";

    private const string DashboardPath = "service/application/dashboard";
    private const string RegisterDevicePath = "service/users/client";
    private const string ControlDevicePath = "service/devices/{0}/control-sync";
    private const string RegisterIoTCertificatePath = "service/users/client/certificate";

    private readonly HttpClient _httpClient;
    private readonly Session _session;

    public ThinQClient(Session session)
    {
        _httpClient = new HttpClient(new AuthenticationMessageHandler(session));
        _session = session;
        _httpClient.BaseAddress = _session.ThinQUri;
    }

    public async Task<GetDevicesResponse> GetDevices()
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, DashboardPath);

        requestMessage.Headers.SetThinQv2ApiHeadersWithAuth(
            _session.CountryCode,
            _session.LanguageCode,
            _session.ClientId,
            _session.AccessToken,
            _session.UserNo);

        var response = await _httpClient.SendAsync(requestMessage);

        return await response.ReadContentFromJsonOrThrowAsync<GetDevicesResponse>();
    }

    public async Task<RouteResponse> GetRoute()
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, RouteUri);

        requestMessage.Headers.SetThinQv2ApiHeadersWithAuth(
            _session.CountryCode,
            _session.LanguageCode,
            _session.ClientId,
            _session.AccessToken,
            _session.UserNo);

        var response = await _httpClient.SendAsync(requestMessage);

        return await response.ReadContentFromJsonOrThrowAsync<RouteResponse>();
    }

    public async Task<string> RegisterDevice()
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, RegisterDevicePath);

        requestMessage.Headers.SetThinQv2ApiHeadersWithAuth(
            _session.CountryCode,
            _session.LanguageCode,
            _session.ClientId,
            _session.AccessToken,
            _session.UserNo);

        var response = await _httpClient.SendAsync(requestMessage);

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<RegisterIoTCertificateResponse> RegisterIotCertificate(string csr)
    {
        var httpContent = new StringContent(JsonSerializer.Serialize(new Dictionary<string, string>
        {
            { "csr", csr },
        }),null, "application/json");

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, RegisterIoTCertificatePath)
        {
            Content = httpContent
        };

        requestMessage.Headers.SetThinQv2ApiHeadersWithAuth(
            _session.CountryCode,
            _session.LanguageCode,
            _session.ClientId,
            _session.AccessToken,
            _session.UserNo);;

        var response = await _httpClient.SendAsync(requestMessage);

        return await response.ReadContentFromJsonOrThrowAsync<RegisterIoTCertificateResponse>();
    }


    public async Task TurnOnAc(string deviceId)
    {
        var body = new Dictionary<string, string>
        {
            { "command", "Operation" },
            { "dataKey", "airState.operation" },
            { "dataValue", "1" },
            { "ctrlKey", "basicCtrl" }
        };

        var httpContent = new StringContent(JsonSerializer.Serialize(body), null, "application/json");

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, string.Format(ControlDevicePath, deviceId))
        {
            Content = httpContent,
        };

        requestMessage.Headers.SetThinQv2ApiHeadersWithAuth(
            _session.CountryCode,
            _session.LanguageCode,
            _session.ClientId,
            _session.AccessToken,
            _session.UserNo);

        await _httpClient.SendAsync(requestMessage);
    }

    public async Task TurnOffAc(string deviceId)
    {
        var body = new Dictionary<string, string>
        {
            { "command", "Operation" },
            { "dataKey", "airState.operation" },
            { "dataValue", "0" },
            { "ctrlKey", "basicCtrl" }
        };

        var httpContent = new StringContent(JsonSerializer.Serialize(body), null, "application/json");

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, string.Format(ControlDevicePath, deviceId))
        {
            Content = httpContent,
        };

        requestMessage.Headers.SetThinQv2ApiHeadersWithAuth(
            _session.CountryCode,
            _session.LanguageCode,
            _session.ClientId,
            _session.AccessToken,
            _session.UserNo);

        await _httpClient.SendAsync(requestMessage);
    }
}
