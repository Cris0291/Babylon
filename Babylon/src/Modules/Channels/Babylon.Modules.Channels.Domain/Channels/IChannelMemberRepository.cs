namespace Babylon.Modules.Channels.Domain.Channels;
public interface IChannelMemberRepository
{
    Task AddChannelMembers(ChannelMember member);
    Task DeleteChannelMember(ChannelMember channelMember);
    Task<ChannelMember?> GetChannelMember(Guid channelId, Guid id);
}
