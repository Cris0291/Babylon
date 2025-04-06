using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace Babylon.Modules.Users.Infrastructure.Identity;
internal sealed class KeyCloakAuthDelegatingHandler(IOptions<KeyCloakOptions> options) : DelegatingHandler
{
    private readonly KeyCloakOptions _options = options.Value;
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        AuthToken authToken = await GetAuthorizationToken(cancellationToken);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken.AccesToken);

        HttpResponseMessage httpResponse = await base.SendAsync(request, cancellationToken);

        httpResponse.EnsureSuccessStatusCode();

        return httpResponse;
    }
    private async Task<AuthToken> GetAuthorizationToken(CancellationToken cancellationToken)
    {
        KeyValuePair<string, string>[] authRequestParameters = 
            [
            new("client_id", _options.ConfidentialClientId),
            new("client_secret", _options.ConfidentialClientSecret),
            new("scope", "openid"),
            new("grant_type", "client_credentials")
            ];

        using var authContent = new FormUrlEncodedContent(authRequestParameters);

        using var authRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(_options.TokenUrl));

        authRequest.Content = authContent;

        HttpResponseMessage httpResponse = await base.SendAsync(authRequest, cancellationToken);

        httpResponse.EnsureSuccessStatusCode();

        return await httpResponse.Content.ReadFromJsonAsync<AuthToken>(cancellationToken);
    }
    internal sealed class AuthToken
    {
        [JsonPropertyName("access_token")]
        public string AccesToken { get; set; }
    }
}
