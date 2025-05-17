namespace Babylon.Modules.Channels.Domain.ThreadChannels;
public class ThreadChannelMember
{
    private ThreadChannelMember() { }
    public Guid ThreadChannelId { get; private set; }
    public Guid Id { get; private set; }
    public static ThreadChannelMember Create(Guid id, Guid threadId)
    {
        return new ThreadChannelMember
        {
            Id = id,
            ThreadChannelId = threadId
        };
    }
}
