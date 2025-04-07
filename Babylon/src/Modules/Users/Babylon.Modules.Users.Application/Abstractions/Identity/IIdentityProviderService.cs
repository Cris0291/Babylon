using Babylon.Common.Domain;

namespace Babylon.Modules.Users.Application.Abstractions.Identity;
public interface IIdentityProviderService
{
    Task<Result<string>> RegisterUserAsync(string email, string firstName, string lastName, string password, CancellationToken cancellationToken);
}
