using Babylon.Modules.Channels.Domain.Members;
using Babylon.Modules.Channels.Domain.MessageChannels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Babylon.Modules.Channels.Infrastructure.MessageChannels;
internal sealed class MessageChannelReactionConfiguration : IEntityTypeConfiguration<MessageChannelReaction>
{
    public void Configure(EntityTypeBuilder<MessageChannelReaction> builder)
    {
        builder.HasKey(x => new {x.MessageChannelId, x.Id});

        builder.HasOne<MessageChannel>()
            .WithMany()
            .HasForeignKey(x => x.MessageChannelId);

        builder.HasOne<Member>()
            .WithMany()
            .HasForeignKey(x => x.Id);
    }
}
