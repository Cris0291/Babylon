using Babylon.Modules.Channels.Domain.Members;
using Babylon.Modules.Channels.Domain.ThreadChannels;

namespace Babylon.Modules.Channels.Domain.Channels;

public sealed class Channel
{
    private Channel() { }
    public Guid ChannelId { get; private set; }
    public string Name { get; private set; }
    public bool PublicChannel { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid Creator { get; private set; } 
    public List<Member> Participants { get; private set; }
    public List<ThreadChannel> Threads { get; private set; }
    public static Channel CreateChannel(string channelName, bool publicChannel, Guid creator)
    {
        return new Channel { Name = channelName, PublicChannel = publicChannel, Creator = creator ,CreatedAt = DateTime.Now};
    }
}

