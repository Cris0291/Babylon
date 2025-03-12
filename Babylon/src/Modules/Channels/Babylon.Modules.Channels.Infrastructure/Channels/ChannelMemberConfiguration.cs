using Babylon.Modules.Channels.Domain.Channels;
using Babylon.Modules.Channels.Domain.Members;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Babylon.Modules.Channels.Infrastructure.Channels;
internal sealed class ChannelMemberConfiguration : IEntityTypeConfiguration<ChannelMember>
{
    public void Configure(EntityTypeBuilder<ChannelMember> builder)
    {
        builder.HasKey(x => new {x.MemberId, x.ChannelId});

        builder.HasOne<Channel>()
            .WithMany()
            .HasForeignKey(x => x.ChannelId);

        builder.HasOne<Member>()
            .WithMany()
            .HasForeignKey(x => x.MemberId);
    }
}
