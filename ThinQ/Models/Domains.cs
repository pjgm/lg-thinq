namespace ThinQ.Models;

public class Domains
{
    private const string Protocol = "https";
    private const string LgThinQSld = "lgthinq.com";
    private const string LgThinQRouteSubdomain = "route";
    private const int LgThinQGatewayPort = 46030;

    public static Uri GatewayBaseAddress =>
        new($"{Protocol}://{LgThinQRouteSubdomain}.{LgThinQSld}:{LgThinQGatewayPort}/v1/service/application/");

    public Domains(string spxUri, string empTermsUri, string thinQ2Uri)
    {
        SpxUri = new Uri($"{spxUri}/");
        EmpTermsUri = new Uri($"{empTermsUri}/");
        ThinQ2Uri = new Uri($"{thinQ2Uri}/");
    }

    public Uri SpxUri { get; set; }
    public Uri EmpTermsUri { get; set; }
    public Uri ThinQ2Uri { get; set; }
}
