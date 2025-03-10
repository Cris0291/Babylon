using Microsoft.EntityFrameworkCore;

namespace Babylon.Modules.Channels.Infrastructure.Database;
public sealed class ChannelsDbContext(DbContextOptions<ChannelsDbContext> options): DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Channels);
    }
}
