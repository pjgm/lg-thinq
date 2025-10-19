using System.Text.Json;
using ThinQ.Configuration;
using ThinQ.Models;
using ThinQ.Services;

namespace ThinQ.SessionManagement;

public record Session(string PersonalAccessToken, string CountryCode, Uri ThinqApiUri);
public class SessionManager(IUserConfigReader userConfigReader)
{
    private const string SessionFilename = "session.json";

    public async Task<Session> GetOrCreate()
    {
        if (File.Exists(SessionFilename))
        {
            return GetExistingSession();
        }

        var userConfig = new UserConfig(
            userConfigReader.ReadCountryCode(),
            userConfigReader.ReadPersonalAccessToken());

        var routeService = new RouteService(userConfig.PersonalAccessToken, userConfig.CountryCode);
        var routeResponse = await routeService.GetRoute();

        var session = new Session(userConfig.PersonalAccessToken, userConfig.CountryCode,
            routeResponse.Response.ApiServer);
        SaveToDisk(session);
        return session;
    }

    private static Session GetExistingSession() =>
        JsonSerializer.Deserialize<Session>(File.ReadAllText(SessionFilename))
        ?? throw new JsonException();

    private static void SaveToDisk(Session session) =>
        File.WriteAllText(SessionFilename, JsonSerializer.Serialize(session));


}
