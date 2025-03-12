using Babylon.Modules.Channels.Domain.Channels;
using Babylon.Modules.Channels.Domain.MessageChannels;
using Babylon.Modules.Channels.Domain.ThreadChannels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Babylon.Modules.Channels.Infrastructure.Channels;
internal sealed class ChannelConfiguration : IEntityTypeConfiguration<Channel>
{
    public void Configure(EntityTypeBuilder<Channel> builder)
    {
        builder.HasMany<ThreadChannel>().WithOne().HasForeignKey(x => x.ChannelId);
        builder.HasMany<MessageChannel>().WithOne().HasForeignKey(x => x.ChannelId);
    }
}
