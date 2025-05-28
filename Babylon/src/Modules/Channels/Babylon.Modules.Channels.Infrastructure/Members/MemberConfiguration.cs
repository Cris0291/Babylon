using Babylon.Modules.Channels.Domain.Members;
using Babylon.Modules.Channels.Domain.MessageChannels;
using Babylon.Modules.Channels.Domain.MessageThreadChannels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Babylon.Modules.Channels.Infrastructure.Members;
internal sealed class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasMany<MessageChannel>().WithOne().HasForeignKey(x => x.Id);
        builder.HasMany<MessageThreadChannel>().WithOne().HasForeignKey(x => x.Id);
    }
}
