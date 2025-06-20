namespace Babylon.Modules.Channels.Domain.ThreadChannels;
public interface IThreadChannelRepository
{
    Task Insert(ThreadChannel threadChannel);
    Task<ThreadChannel?> Get(Guid threadChannelId);
}
