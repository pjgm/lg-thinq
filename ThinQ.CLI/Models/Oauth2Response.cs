namespace ThinQ.CLI.Models;

public class Oauth2Response
{
    public string access_token { get; set; }
    public string expires_in { get; set; }
    public string refresh_token { get; set; }
    public string oauth2_backend_url { get; set; }
}
