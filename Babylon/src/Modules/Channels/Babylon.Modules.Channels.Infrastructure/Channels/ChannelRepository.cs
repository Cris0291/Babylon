using Babylon.Modules.Channels.Domain.Channels;
using Babylon.Modules.Channels.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Babylon.Modules.Channels.Infrastructure.Channels;
internal sealed class ChannelRepository(ChannelsDbContext dbContext) : IChannelRepository
{
    public async Task<bool> Exist(Guid channelId)
    {
        Channel? channel = await dbContext.Channels.SingleOrDefaultAsync(c => c.ChannelId == channelId);
        return channel != null;
    }

    public async Task Insert(Channel channel)
    {
        await dbContext.AddAsync(channel);
    }
    public async Task<Channel?> GetChannel(Guid channelId)
    {
        return await dbContext.Channels.SingleOrDefaultAsync(c => c.ChannelId == channelId);
    }

    public async Task Delete(Channel channel)
    {
        dbContext.Remove(channel);
        await Task.CompletedTask;
    }
}
