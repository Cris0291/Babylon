using Babylon.Common.Application.EventBus;

namespace Babylon.Modules.Channels.IntegrationEvents;

public sealed class ChannelPublishMessageIntegrationEvent(
    Guid channelId,
    Guid userId,
    string message,
    DateTime publicationDate,
    string userName,
    string avatarUrl)
    : IntegrationEvent(Guid.NewGuid(), DateTime.Now)
{
    public Guid ChannelId { get; init; } = channelId;
    public Guid UserId { get; init; } = userId;
    public string Message { get; init; } = message;
    public DateTime PublicationDate { get; init; } = publicationDate;
    public string UserName { get; init; } = userName;
    public string AvatarUrl { get; init; } = avatarUrl;
}
    

