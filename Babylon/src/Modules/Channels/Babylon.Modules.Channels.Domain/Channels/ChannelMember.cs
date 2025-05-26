namespace Babylon.Modules.Channels.Domain.Channels;
public sealed class ChannelMember
{
    private ChannelMember() { }
    public Guid ChannelId { get; private set; }
    public Guid Id { get; private set; }
    public bool IsMute { get; private set; }
    public static ChannelMember Create(Guid channelId, Guid id)
    {
        return new ChannelMember
        {
            ChannelId = channelId,
            Id = id,
        };
    }
    public void MuteMember()
    {
        IsMute = true;
    }
}
