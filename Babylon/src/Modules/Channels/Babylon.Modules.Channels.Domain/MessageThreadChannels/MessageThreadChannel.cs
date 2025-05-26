namespace Babylon.Modules.Channels.Domain.MessageThreadChannels;

public sealed class MessageThreadChannel
{
    private MessageThreadChannel() { }
    public Guid MessageThreadChannelId { get; private set; }
    public string UserName { get; private set; }
    public string Message { get; private set; }
    public DateTime PublicationDate { get; private set; }
    public Guid ThreadChannelId { get; private set; }
    public Guid Id { get; private set; }
    public static MessageThreadChannel Create(string userName, string message, Guid threadId, Guid id, DateTime creationDate = default)
    {
        return new MessageThreadChannel
        {
            UserName = userName,
            Message = message,
            PublicationDate = creationDate != default ? creationDate : DateTime.Now,
            ThreadChannelId = threadId,
            Id = id
        };
    }
}
    
