using Babylon.Modules.Channels.Domain.DirectedChannels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Babylon.Modules.Channels.Infrastructure.DirectedChannels;

public class DirectedChannelMemberConfiguration : IEntityTypeConfiguration<DirectedChannelMember>
{
    public void Configure(EntityTypeBuilder<DirectedChannelMember> builder)
    {
        builder.HasKey(x => new {x.Id, x.DirectedChannelId});
    }
}
