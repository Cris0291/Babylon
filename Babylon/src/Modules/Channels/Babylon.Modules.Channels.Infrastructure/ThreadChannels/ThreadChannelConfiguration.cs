using Babylon.Modules.Channels.Domain.MessageThreadChannels;
using Babylon.Modules.Channels.Domain.ThreadChannels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Babylon.Modules.Channels.Infrastructure.ThreadChannels;
internal sealed class ThreadChannelConfiguration : IEntityTypeConfiguration<ThreadChannel>
{
    public void Configure(EntityTypeBuilder<ThreadChannel> builder)
    {
        builder.HasMany<MessageThreadChannel>().WithOne().HasForeignKey(x => x.ThreadChannelId);
    }
}
