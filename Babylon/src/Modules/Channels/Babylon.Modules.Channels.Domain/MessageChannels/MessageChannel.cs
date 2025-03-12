namespace Babylon.Modules.Channels.Domain.MessageChannels;

public sealed class MessageChannel
{
    public Guid MessageChannelId { get; private set; }
    public string Name { get; private set; }
    public Guid ChannelId { get; private set; }
    public Guid MemberId { get; private set; }
}
    

