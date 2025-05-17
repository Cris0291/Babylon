namespace Babylon.Modules.Channels.Domain.ThreadChannels;
public interface IThreadChannelMemberRepository
{
    Task Insert(IEnumerable<ThreadChannelMember> members);
}
