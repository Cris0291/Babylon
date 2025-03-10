namespace Babylon.Modules.Channels.Domain.MessageChannel;

public sealed class MessageChannel
{
    private MessageChannel() { }
    public Guid MessageChannelId { get; private set; }
    public string Name { get; private set; }
}
    

