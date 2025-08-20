using Babylon.Modules.Channels.Domain.Members;
using Babylon.Modules.Channels.Domain.MessageDIrectedChannels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Babylon.Modules.Channels.Infrastructure.MeesageDirectedChannels;

internal sealed class MessageDirectedChannelReactionConfiguration : IEntityTypeConfiguration<MessageDirectedChannelReaction>
{
    public void Configure(EntityTypeBuilder<MessageDirectedChannelReaction> builder)
    {
        builder.HasKey(x => new { x.Id, x.MessageDirectedChannelId});

        builder.HasOne<MessageDirectedChannel>()
            .WithMany()
            .HasForeignKey(x => x.MessageDirectedChannelId);

        builder.HasOne<Member>()
            .WithMany()
            .HasForeignKey(x => x.Id);
    }
}
