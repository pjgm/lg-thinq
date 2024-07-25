﻿using ThinQ.CLI.Configuration;
using ThinQ.CLI.HttpClients;
using ThinQ.CLI.Models;
using ThinQ.CLI.SessionManagement;

namespace ThinQ.CLI.Services;

public class AuthenticationFlowService(Domains domains)
{
    private Domains Domains { get; } = domains;

    private readonly SpxHttpClient _spxClient = new(new HttpClient(), domains.SpxUri);
    private readonly EmpTermsHttpClient _empTermsClient = new(new HttpClient(), domains.EmpTermsUri);
    private readonly EmpOAuthHttpClient _empOAuthClient = new(new HttpClient());

    public async Task<Session> LoginFlow(UserConfig userConfig)
    {
        var (username, password, countryCode, languageCode, clientId) = userConfig;

        var (signature, timestamp, encryptedPassword) = await _spxClient.PreLogin(username, password);

        var loginResponse = await _empTermsClient
            .Login(countryCode, languageCode, username, signature, timestamp, encryptedPassword);

        var secretOAuthKey = await _spxClient
            .GetSecretOauthKey();

        var authorizeEmpResponse = await _empOAuthClient
            .AuthorizeEmp(loginResponse.UserId, loginResponse.UserIdType, loginResponse.Country, secretOAuthKey.ReturnData, loginResponse.LoginSessionId);

        var (tokenResponse, userProfile) = await OAuthFlow(authorizeEmpResponse.OAuth2Url, authorizeEmpResponse.Code);
        return new Session(userProfile, tokenResponse, countryCode, languageCode, clientId, Domains.ThinQ2Uri);
    }

    private static async Task<(Oauth2Response, UserProfileResponse)> OAuthFlow(string authorizationServerBaseAddress, string code)
    {
        var httpClient = new HttpClient();
        var baseUri = new Uri(authorizationServerBaseAddress);
        var authorizationServerHttpClient = new AuthorizationServerHttpClient(httpClient, baseUri);

        var tokenResponse = await authorizationServerHttpClient
            .GetOAuthToken(code);

        var userProfile = await authorizationServerHttpClient
            .GetUserProfile(tokenResponse.access_token);

        return (tokenResponse, userProfile);
    }
}