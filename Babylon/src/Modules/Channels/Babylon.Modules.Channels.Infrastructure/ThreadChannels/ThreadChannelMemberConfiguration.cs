using Babylon.Modules.Channels.Domain.Members;
using Babylon.Modules.Channels.Domain.ThreadChannels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Babylon.Modules.Channels.Infrastructure.ThreadChannels;
internal sealed class ThreadChannelMemberConfiguration : IEntityTypeConfiguration<ThreadChannelMember>
{
    public void Configure(EntityTypeBuilder<ThreadChannelMember> builder)
    {
        builder.HasKey(x => new {x.ThreadChannelId, x.Id});

        builder.HasOne<ThreadChannel>()
            .WithMany()
            .HasForeignKey(x => x.ThreadChannelId);

        builder.HasOne<Member>()
            .WithMany()
            .HasForeignKey(x => x.Id);
    }
}
