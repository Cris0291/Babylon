using Babylon.Modules.Channels.Domain.Channels;
using Babylon.Modules.Channels.Domain.Members;
using Babylon.Modules.Channels.Domain.MessageChannels;
using Babylon.Modules.Channels.Domain.MessageThreadChannels;
using Babylon.Modules.Channels.Domain.ThreadChannels;
using Microsoft.EntityFrameworkCore;

namespace Babylon.Modules.Channels.Infrastructure.Database;
public sealed class ChannelsDbContext(DbContextOptions<ChannelsDbContext> options): DbContext(options)
{
    internal DbSet<Channel> Channels { get; set; }
    internal DbSet<Member> Members { get; set; }
    internal DbSet<MessageChannel> MesssageChannels { get; set; }
    internal DbSet<MessageThreadChannel> MessageThreadChannels { get; set; }
    internal DbSet<ThreadChannel> ThreadChannels { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Channels);
    }
}
