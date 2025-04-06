using System.Net.Http.Json;

namespace Babylon.Modules.Users.Infrastructure.Identity;
internal sealed class KeyCloakClient(HttpClient httpClient)
{
    internal async Task<string> RegisterUserAsync(UserRepresentation user, CancellationToken cancellationToken)
    {
        HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync(
            "users",
            user,
            cancellationToken
            );

        return ExtractIdentityFromLocationHeader(httpResponseMessage);
    }
    private static string ExtractIdentityFromLocationHeader(HttpResponseMessage httpResponseMessage)
    {
        const string userSegment = "users/";
        string? location = httpResponseMessage.Headers.Location?.PathAndQuery;

        if(location == null)
        {
            throw new InvalidOperationException("Location header is null");
        }

        int indexLocation = location.IndexOf(userSegment, StringComparison.InvariantCultureIgnoreCase);
        return location.Substring(indexLocation + location.Length);
    }
}
