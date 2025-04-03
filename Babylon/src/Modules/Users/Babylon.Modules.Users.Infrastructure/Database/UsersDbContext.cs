using Microsoft.EntityFrameworkCore;

namespace Babylon.Modules.Users.Infrastructure.Database;
public sealed class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options)
{

}
