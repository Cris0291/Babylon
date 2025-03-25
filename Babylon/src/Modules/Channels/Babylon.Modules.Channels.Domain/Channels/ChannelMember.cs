namespace Babylon.Modules.Channels.Domain.Channels;
public sealed class ChannelMember
{
    private ChannelMember() { }
    public Guid ChannelId { get; private set; }
    public Guid MemberId { get; private set; }
    public static ChannelMember Create(Guid channelId, Guid memberId)
    {
        return new ChannelMember
        {
            ChannelId = channelId,
            MemberId = memberId,
        };
    }
}
