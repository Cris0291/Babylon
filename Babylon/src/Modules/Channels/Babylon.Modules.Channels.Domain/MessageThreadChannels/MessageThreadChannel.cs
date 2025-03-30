namespace Babylon.Modules.Channels.Domain.MessageThreadChannels;

public sealed class MessageThreadChannel
{
    private MessageThreadChannel() { }
    public Guid MessageThreadChannelId { get; private set; }
    public string UserName { get; private set; }
    public string MessageText { get; private set; }
    public DateTime CreationDate { get; private set; }
    public Guid ThreadChannelId { get; private set; }
    public Guid MemberId { get; private set; }
    public static MessageThreadChannel Create(string userName, string message, Guid threadId, Guid memberId, DateTime creationDate = default)
    {
        return new MessageThreadChannel
        {
            UserName = userName,
            MessageText = message,
            CreationDate = creationDate != default ? creationDate : DateTime.Now,
            ThreadChannelId = threadId,
            MemberId = memberId
        };
    }
}
    
