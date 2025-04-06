using System.Net;
using Babylon.Common.Domain;
using Babylon.Modules.Users.Application.Abstractions.Identity;

namespace Babylon.Modules.Users.Infrastructure.Identity;
internal sealed class IdentityProviderService(KeyCloakClient keyCloakClient) : IIdentityProviderService
{
    private const string PasswordCredentialType = "Password";
    public async Task<Result<string>> RegisterUserAsync(string email, string firstName, string lastName, string password, CancellationToken cancellationToken)
    {
        var userRepresentation = new UserRepresentation(
            email,
            email,
            firstName,
            lastName,
            true,
            true,
            [new CredentialRepresentation(PasswordCredentialType, password, false)]
            );

        try
        {
            string userIdentity = await keyCloakClient.RegisterUserAsync(userRepresentation, cancellationToken);

            return userIdentity;
        }
        catch(HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.Conflict)
        {
            return Result.Failure<string>(Error.Failure(description: "User registration failed. Email waas not unique"));
        }
    }
}
