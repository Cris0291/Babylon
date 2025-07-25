namespace Babylon.Modules.Channels.Domain.DirectedChannels;

public interface IDirectedChannelRepository
{
    Task<DirectedChannel?> Get(Guid creator);
}
