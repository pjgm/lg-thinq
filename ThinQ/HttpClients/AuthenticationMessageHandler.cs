using ThinQ.SessionManagement;

namespace ThinQ.HttpClients;

internal class AuthenticationMessageHandler(Session session) : DelegatingHandler(new HttpClientHandler())
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);
        var authClient = new AuthorizationServerHttpClient(new HttpClient(), new Uri(Uri.UnescapeDataString(session.Oauth2.oauth2_backend_url)));

        if (response.IsSuccessStatusCode)
        {
            return response;
        }

        // JWT is possibly expired, refresh it
        var newTokens = await authClient.RefreshToken(session.RefreshToken);
        session.Oauth2.access_token = newTokens.access_token;

        // Retry the request
        request.Headers.Remove("x-emp-token");
        request.Headers.Add("x-emp-token", newTokens.access_token);
        response = await base.SendAsync(request, cancellationToken);

        return response;
    }
}
