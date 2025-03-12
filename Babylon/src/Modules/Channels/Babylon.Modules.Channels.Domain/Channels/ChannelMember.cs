namespace Babylon.Modules.Channels.Domain.Channels;
public sealed class ChannelMember
{
    public Guid ChannelId { get; private set; }
    public Guid MemberId { get; private set; }
}
