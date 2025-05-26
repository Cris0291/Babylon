using Babylon.Common.Domain;

namespace Babylon.Modules.Channels.Domain.MessageChannels;

public sealed class MessageChannel : Entity
{
    private MessageChannel() { }
    public Guid MessageChannelId { get; private set; }
    public string UserName { get; private set; }
    public string Message { get; private set; }
    public string AvatarUrl { get; private set; }
    public int Like { get; private set; }
    public int Dislike { get; private set; }
    public DateTime PublicationDate { get; private set; }
    public Guid ChannelId { get; private set; }
    public Guid Id { get; private set; }
    public static MessageChannel Create(
        Guid channelId, 
        Guid id, 
        string message, 
        DateTime publicationDate, 
        string userName, 
        string avatar)
    {
        return new MessageChannel
        {
            ChannelId = channelId,
            Id = id,
            Message = message,
            PublicationDate = publicationDate,
            UserName = userName,
            AvatarUrl = avatar,
            Like = 0,
            Dislike = 0,
        };
    }
}
    

