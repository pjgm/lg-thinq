﻿using System.Net.Http.Headers;
using System.Net.Mime;
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

    public ThinQClient(Session session)
    {
        _httpClient = new HttpClient(new AuthenticationMessageHandler(session));
        _httpClient.BaseAddress = session.ThinQUri;
        _httpClient.DefaultRequestHeaders.SetThinQv2ApiHeadersWithAuth(session);
    }

    public async Task<GetDevicesResponse> GetDevices()
    {
        var response = await _httpClient.GetAsync(DashboardPath);

        return await response.ReadContentFromJsonOrThrowAsync<GetDevicesResponse>();
    }

    public async Task<RouteResponse> GetRoute()
    {
        var response = await _httpClient.GetAsync(RouteUri);

        return await response.ReadContentFromJsonOrThrowAsync<RouteResponse>();
    }

    public async Task<string> RegisterDevice()
    {
        var response = await _httpClient.PostAsync(RegisterDevicePath, null);

        return await response.Content.ReadAsStringAsync();
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

    public async Task TurnOnAc(string deviceId)
    {
        var httpContent = BuildCommandHttpContent(OperationDataKeyPair.AcOn);

        await _httpClient.PostAsync(string.Format(ControlDevicePath, deviceId), httpContent);
    }

    public async Task TurnOffAc(string deviceId)
    {
        var httpContent = BuildCommandHttpContent(OperationDataKeyPair.AcOff);

        await _httpClient.PostAsync(string.Format(ControlDevicePath, deviceId), httpContent);
    }

    public async Task SetTemperature(string deviceId, int temperature)
    {
        var httpContent = BuildCommandHttpContent(OperationDataKeyPair.AcTemperature(temperature));

        await _httpClient.PostAsync(string.Format(ControlDevicePath, deviceId), httpContent);
    }

    private HttpContent BuildCommandHttpContent((string DataKey, string DataValue) dataKeyPair)
    {
        var body = new Dictionary<string, string>
        {
            { "command", "Operation" },
            { "ctrlKey", "basicCtrl" },
            { "dataKey", dataKeyPair.DataKey },
            { "dataValue", dataKeyPair.DataValue }
        };

        return new StringContent(JsonSerializer.Serialize(body), null, MediaTypeNames.Application.Json);
    }
}

public static class OperationDataKeyPair
{
    private const string AcOperationDataKey = "airState.operation";
    private const string AcTemperatureDataKey = "airState.tempState.target";

    public static (string DataKey, string DataValue) AcOff => (AcOperationDataKey, "0");
    public static (string DataKey, string DataValue) AcOn => (AcOperationDataKey, "1");
    public static (string DataKey, string DataValue) AcTemperature(int temp) => (AcTemperatureDataKey, temp.ToString());
}

public static class ThinQHttpRequestHeadersExtensions
{
    public static void SetThinQv2ApiHeadersWithAuth(this HttpRequestHeaders headers, Session session) =>
        headers.AddRange(GetAuthorizedThinQv2Headers(session));

    public static void SetThinQv2ApiHeaders(this HttpRequestHeaders headers, string countryCode, string languageCode,
        string clientId) =>
        headers.AddRange(GetThinQv2Headers(countryCode, languageCode, clientId));

    private static IEnumerable<(string, string)> GetAuthorizedThinQv2Headers(Session session)
    {
        var apiHeaders = GetThinQv2Headers(session.ClientId, session.CountryCode, session.LanguageCode).ToList();
        apiHeaders.Add(("Authorization", $"Bearer {session.AccessToken}"));
        apiHeaders.Add(("x-emp-token", session.AccessToken));
        apiHeaders.Add(("x-user-no", session.UserNo));
        apiHeaders.Add(("country_code", session.CountryCode));
        apiHeaders.Add(("language_code", session.LanguageCode));
        return apiHeaders;
    }

    private static IEnumerable<(string, string)> GetThinQv2Headers(string clientId, string countryCode,
        string languageCode)
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
}
