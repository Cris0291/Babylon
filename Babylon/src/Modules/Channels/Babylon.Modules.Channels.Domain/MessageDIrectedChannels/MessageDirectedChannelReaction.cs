namespace Babylon.Modules.Channels.Domain.MessageDIrectedChannels;

public class MessageDirectedChannelReaction
{
    private MessageDirectedChannelReaction() { }
    public Guid Id { get; private set; }
    public Guid  MessageDirectedChannelId { get; private set; }
    public string? Emoji { get; private set; }
    public bool? Like { get; private set; }
    public bool? Dislike { get; private set; }
    public bool Pin { get; private set; }
}
