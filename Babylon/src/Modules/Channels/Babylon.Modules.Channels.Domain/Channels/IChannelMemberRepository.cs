namespace Babylon.Modules.Channels.Domain.Channels;
public interface IChannelMemberRepository
{
    Task AddChannelMembers(IEnumerable<ChannelMember> members);
}
