using Babylon.Modules.Channels.Domain.ThreadChannels;
using Babylon.Modules.Channels.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Babylon.Modules.Channels.Infrastructure.ThreadChannels;
internal sealed class ThreadChannelRepository(ChannelsDbContext dbContext) : IThreadChannelRepository
{
    public async Task Insert(ThreadChannel threadChannel)
    {
        await dbContext.AddAsync(threadChannel);
    }

    public async Task<ThreadChannel?> Get(Guid threadChannelId)
    {
        return await dbContext.ThreadChannels.SingleOrDefaultAsync(x => x.ThreadChannelId == threadChannelId);
    }
}
