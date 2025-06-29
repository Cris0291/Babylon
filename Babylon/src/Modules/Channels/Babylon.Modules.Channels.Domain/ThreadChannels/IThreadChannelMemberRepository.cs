namespace Babylon.Modules.Channels.Domain.ThreadChannels;
public interface IThreadChannelMemberRepository
{
    Task Insert(IEnumerable<ThreadChannelMember> members);
    Task<ThreadChannelMember?> Get(Guid id, Guid threadChannelId);
    Task DeleteThreadChannelMember(ThreadChannelMember member);
}
