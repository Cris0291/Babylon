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

        builder
            .HasMany(m => m.BlockedMembers)
            .WithMany(m => m.BlockedByMembers)
            .UsingEntity<Dictionary<string, object>>(
                "MemberBlock",
                right => right
                    .HasOne<Member>()
                    .WithMany()
                    .HasForeignKey("BlockerId")
                    .OnDelete(DeleteBehavior.Restrict),
                left => left
                    .HasOne<Member>()
                    .WithMany()
                    .HasForeignKey("BlockedId")
                    .OnDelete(DeleteBehavior.Restrict),
                join =>
                {
                    join.HasKey("BlockerId", "BlockedId");
                    join.Property<DateTime>("CreatedAt")
                        .HasDefaultValueSql("SYSUTCDATETIME()");
                }
            );
    }
}
