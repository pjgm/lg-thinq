namespace ThinQ.Models;

public class RouteResponse : BaseApiResponse
{
    public RouteResponseEndpointsResult Response { get; set; }
}

public class RouteResponseEndpointsResult
{
    public Uri ApiServer { get; set; }
    public Uri MqttServer { get; set; }
    public Uri WebSocketServer { get; set; }
}
