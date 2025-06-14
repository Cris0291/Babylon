using Babylon.Modules.Channels.Domain.Members;
using Babylon.Modules.Channels.Domain.MessageThreadChannels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Babylon.Modules.Channels.Infrastructure.MessageThreadChannels;

internal sealed class MessageThreadChannelReactionConfiguration : IEntityTypeConfiguration<MessageThreadChannelReaction>
{
    public void Configure(EntityTypeBuilder<MessageThreadChannelReaction> builder)
    {
        builder.HasKey(x => new { x.Id, x.MessageThreadChannelId});

        builder.HasOne<MessageThreadChannel>()
            .WithMany()
            .HasForeignKey(x => x.MessageThreadChannelId);

        builder.HasOne<Member>()
            .WithMany()
            .HasForeignKey(x => x.Id);
    }
}
