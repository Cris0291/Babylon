using Babylon.Common.Application.EventBus;

namespace Babylon.Modules.Channels.IntegrationEvents;
public sealed class ThreadPublishMessageIntegrationEvent(
    Guid threadId,
    Guid memberId,
    string message,
    DateTime publicationDate,
    string userName,
    string avatarUrl)
    : IntegrationEvent(Guid.NewGuid(), DateTime.Now)
{
    public Guid ThreadId { get; init; } = threadId;
    public Guid MemberId { get; init; } = memberId;
    public string Message { get; init; } = message;
    public DateTime PublicationDate { get; init; } = publicationDate;
    public string UserName { get; init; } = userName;
    public string AvatarUrl { get; init; } = avatarUrl;
}
