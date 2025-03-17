using Babylon.Modules.Channels.Domain.Channels;
using Babylon.Modules.Channels.Infrastructure.Database;

namespace Babylon.Modules.Channels.Infrastructure.Channels;
internal sealed class ChannelRepository(ChannelsDbContext channelsDbContext) : IChannelRepository
{
    public Task Insert(Channel channel)
    {
        throw new NotImplementedException();
    }
}
