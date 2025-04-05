using Babylon.Modules.Users.Domain.Users;
using Babylon.Modules.Users.Infrastructure.Database;

namespace Babylon.Modules.Users.Infrastructure.Users;
internal sealed class UserRepository(UsersDbContext dbContext) : IUserRepository
{
    public async Task Insert(User user)
    {
        await dbContext.AddAsync(user);
    }
}
