using Babylon.Modules.Channels.Domain.Members;

namespace Babylon.Modules.Channels.Domain.ThreadChannels;

public sealed class ThreadChannel
{
    public Guid ThreadChannelId { get; private set; }
    public string ChannelName { get; private set; }
    public Guid ChannelId { get; private set; }
    public Guid FirstMessage { get; private set; }
}
    

