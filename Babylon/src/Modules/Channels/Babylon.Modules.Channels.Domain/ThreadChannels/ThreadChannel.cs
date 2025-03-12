using Babylon.Modules.Channels.Domain.Members;

namespace Babylon.Modules.Channels.Domain.ThreadChannels;

public sealed class ThreadChannel
{
    public Guid ThreadChannelId { get; private set; }
    public string Name { get; private set; }
    public Guid ChannelId { get; private set; }
    public Guid Creator { get; private set; }
    public List<Member> Participants { get; private set; }
}
    

