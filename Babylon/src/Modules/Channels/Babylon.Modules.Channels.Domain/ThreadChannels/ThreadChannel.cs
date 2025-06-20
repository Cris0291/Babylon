using Babylon.Modules.Channels.Domain.Members;

namespace Babylon.Modules.Channels.Domain.ThreadChannels;

public sealed class ThreadChannel
{
    private ThreadChannel() { }
    public Guid ThreadChannelId { get; private set; }
    public string ChannelName { get; private set; }
    public Guid ChannelId { get; private set; }
    public static ThreadChannel Create(string channelName, Guid channelId)
    {
        return new ThreadChannel
        {
            ChannelName = channelName,
            ChannelId = channelId,
            ThreadChannelId = Guid.NewGuid()
        };
    }
    public void Rename(string threadChannelName)
    {
        ChannelName = threadChannelName;
    }
}
    

