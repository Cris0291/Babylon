using Babylon.Modules.Channels.Domain.DirectedChannels;
using Babylon.Modules.Channels.Domain.MessageDIrectedChannels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Babylon.Modules.Channels.Infrastructure.DirectedChannels;

internal sealed class DirectedChannelConfiguration : IEntityTypeConfiguration<DirectedChannel>
{
    public void Configure(EntityTypeBuilder<DirectedChannel> builder)
    {
        builder.HasMany<MessageDirectedChannel>().WithOne().HasForeignKey(x => x.DirectedChannelId);
    }
}
