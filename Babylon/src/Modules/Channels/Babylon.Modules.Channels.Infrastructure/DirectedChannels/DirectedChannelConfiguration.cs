using Babylon.Modules.Channels.Domain.DirectedChannels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Babylon.Modules.Channels.Infrastructure.DirectedChannels;

internal sealed class DirectedChannelConfiguration : IEntityTypeConfiguration<DirectedChannel>
{
    public void Configure(EntityTypeBuilder<DirectedChannel> builder)
    {
        throw new NotImplementedException();
    }
}
