using Babylon.Modules.Channels.Domain.Channels;
using Babylon.Modules.Channels.Infrastructure.Database;

namespace Babylon.Modules.Channels.Infrastructure.Channels;
internal sealed class ChannelRepository(ChannelsDbContext dbContext) : IChannelRepository
{
    public async Task Insert(Channel channel)
    {
        await dbContext.AddAsync(channel);
    }
}
