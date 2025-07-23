using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.Channels;
using Babylon.Modules.Channels.Domain.DirectedChannels;
using Babylon.Modules.Channels.Domain.Members;
using Babylon.Modules.Channels.Domain.MessageChannels;
using Babylon.Modules.Channels.Domain.MessageThreadChannels;
using Babylon.Modules.Channels.Domain.ThreadChannels;
using Babylon.Modules.Channels.Infrastructure.Channels;
using Babylon.Modules.Channels.Infrastructure.Members;
using Babylon.Modules.Channels.Infrastructure.MessageChannels;
using Babylon.Modules.Channels.Infrastructure.MessageThreadChannels;
using Babylon.Modules.Channels.Infrastructure.ThreadChannels;
using Microsoft.EntityFrameworkCore;

namespace Babylon.Modules.Channels.Infrastructure.Database;
public sealed class ChannelsDbContext(DbContextOptions<ChannelsDbContext> options): DbContext(options), IUnitOfWork
{
    internal DbSet<Channel> Channels { get; set; }
    internal DbSet<Member> Members { get; set; }
    internal DbSet<ChannelMember> ChannelMembers { get; set; }
    internal DbSet<MessageChannel> MesssageChannels { get; set; }
    internal DbSet<MessageChannelReaction> MessageChannelReactions { get; set; }
    internal DbSet<MessageThreadChannel> MessageThreadChannels { get; set; }
    internal DbSet<ThreadChannel> ThreadChannels { get; set; }
    internal DbSet<ThreadChannelMember> ThreadChannelMembers { get; set; }
    internal DbSet<MessageThreadChannelReaction> MessageThreadChannelReactions { get; set; }
    
    internal DbSet<DirectedChannel> DirectedChannels { get; set; }
    
    internal DbSet<DirectedChannelMember> DirectedChannelMembers { get; set;  }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ChannelConfiguration());
        modelBuilder.ApplyConfiguration(new ChannelMemberConfiguration());
        modelBuilder.ApplyConfiguration(new MemberConfiguration());
        modelBuilder.ApplyConfiguration(new ThreadChannelConfiguration());
        modelBuilder.ApplyConfiguration(new ThreadChannelMemberConfiguration());
        modelBuilder.ApplyConfiguration(new MessageChannelReactionConfiguration());
        modelBuilder.ApplyConfiguration(new MessageThreadChannelReactionConfiguration());
        modelBuilder.HasDefaultSchema(Schemas.Channels);
    }
}
