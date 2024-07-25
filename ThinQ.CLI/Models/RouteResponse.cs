namespace ThinQ.CLI.Models;

public class RouteResponse
{
    public string resultCode { get; set; }
    public RouteResult result { get; set; }
}

public class RouteResult
{
    public string apiServer { get; set; }
    public string mqttServer { get; set; }
}
