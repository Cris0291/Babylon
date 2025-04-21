using Babylon.Common.Infrastructure.Inbox;
using Babylon.Common.Infrastructure.Outbox;
using Babylon.Modules.Users.Application.Abstractions.Data;
using Babylon.Modules.Users.Domain.Users;
using Babylon.Modules.Users.Infrastructure.Users;
using Microsoft.EntityFrameworkCore;

namespace Babylon.Modules.Users.Infrastructure.Database;
public sealed class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options),  IUnitOfWork
{
    internal DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Users);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new PermissionConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConsumerConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageConsumerConfiguration());
    }
}
