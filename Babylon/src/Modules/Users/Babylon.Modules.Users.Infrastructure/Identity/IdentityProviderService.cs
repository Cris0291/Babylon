using Babylon.Modules.Users.Application.Abstractions.Identity;

namespace Babylon.Modules.Users.Infrastructure.Identity;
internal sealed class IdentityProviderService : IIdentityProviderService
{
    public Task<string> RegisterUserAsync(string email, string firstName, string lastName, string password, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
