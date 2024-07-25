using System.Text.Json;
using ThinQ.CLI.Configuration;
using ThinQ.CLI.Models;
using ThinQ.CLI.Services;
using ThinQ.CLI.Ui;

namespace ThinQ.CLI.SessionManagement;

public record Session(
    UserProfileResponse Profile,
    Oauth2Response Oauth2,
    string CountryCode,
    string LanguageCode,
    string ClientId,
    Uri ThinQUri)
{
    public string AccessToken => Oauth2.access_token;
    public string RefreshToken => Oauth2.refresh_token;
    public string UserNo => Profile.account.userNo;
}
public static class SessionManager
{
    private const string SessionFilename = "session.json";

    public static async Task<Session> GetOrCreate()
    {
        if (File.Exists(SessionFilename))
        {
            return GetExistingSession();
        }

        var userConfig = new UserConfig(
            ConsoleInputReader.ReadUsername(),
            ConsoleInputReader.ReadPassword(),
            ConsoleInputReader.ReadCountryCode(),
            ConsoleInputReader.ReadLanguageCode(),
            Guid.NewGuid().ToString());

        var gatewayService = new GatewayService(new HttpClient(), userConfig);
        var gatewayResponse = await gatewayService.GetGateway();

        var domains = new Domains(
            gatewayResponse.Result.EmpSpxUri,
            gatewayResponse.Result.EmpTermsUri,
            gatewayResponse.Result.Thinq2Uri);

        var authenticationFlowService = new AuthenticationFlowService(domains);
        var session =  await authenticationFlowService.LoginFlow(userConfig);
        SaveToDisk(session);
        return session;
    }

    private static Session GetExistingSession() =>
        JsonSerializer.Deserialize<Session>(File.ReadAllText(SessionFilename))
        ?? throw new JsonException();

    private static void SaveToDisk(Session session) =>
        File.WriteAllText(SessionFilename, JsonSerializer.Serialize(session));


}
