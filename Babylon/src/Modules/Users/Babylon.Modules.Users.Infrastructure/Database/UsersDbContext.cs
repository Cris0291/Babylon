using Babylon.Modules.Users.Application.Abstractions.Data;
using Babylon.Modules.Users.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Babylon.Modules.Users.Infrastructure.Database;
public sealed class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options),  IUnitOfWork
{
    internal DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
