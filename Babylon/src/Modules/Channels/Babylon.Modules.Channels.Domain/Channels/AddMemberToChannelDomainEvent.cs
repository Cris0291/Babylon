using Babylon.Common.Domain;

namespace Babylon.Modules.Channels.Domain.Channels;
public sealed class AddMemberToChannelDomainEvent : DomainEvent
{
    public AddMemberToChannelDomainEvent(Guid userId, Guid channelId) : base(Guid.NewGuid(), DateTime.Now)
    {
        ChannelId = channelId;
        UserId = userId;
    }
    public Guid ChannelId { get; init; }
    public Guid UserId { get; init; }
}
