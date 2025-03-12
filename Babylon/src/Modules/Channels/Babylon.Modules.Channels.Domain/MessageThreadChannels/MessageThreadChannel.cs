namespace Babylon.Modules.Channels.Domain.MessageThreadChannels;

public sealed class MessageThreadChannel
{
    public Guid MessageThreadChannelId { get; private set; }
    public string Name { get; private set; }
    public Guid ThreadChannelId { get; private set; }
    public Guid MemberId { get; private set; }
}
    
