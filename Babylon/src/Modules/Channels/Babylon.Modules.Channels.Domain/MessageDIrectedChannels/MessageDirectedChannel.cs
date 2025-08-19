namespace Babylon.Modules.Channels.Domain.MessageDIrectedChannels;

public class MessageDirectedChannel
{
    private MessageDirectedChannel() { }
    public Guid MessageDirectedChannelId { get; private set; }
    public string UserName { get; private set; }
    public string Message { get; private set; }
    public string AvatarUrl { get; private set; }
    public int NumberOfLikes { get; private set; }
    public int NumberOfDislikes { get; private set; }
    public DateTime PublicationDate { get; private set; }
    public Guid DirectedChannelId { get; private set; }
    public Guid Id { get; private set; }
}
