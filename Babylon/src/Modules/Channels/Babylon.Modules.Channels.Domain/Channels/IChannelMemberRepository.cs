namespace Babylon.Modules.Channels.Domain.Channels;
public interface IChannelMemberRepository
{
    Task AddChannelMembers(IEnumerable<ChannelMember> members);
    Task DeleteChannelMember(ChannelMember channelMember);
    Task<ChannelMember> GetChannelMember(Guid channelId, Guid memberId);
}
