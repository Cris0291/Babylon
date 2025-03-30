using Babylon.Modules.Channels.Domain.ThreadChannels;
using Babylon.Modules.Channels.Infrastructure.Database;

namespace Babylon.Modules.Channels.Infrastructure.ThreadChannels;
internal sealed class ThreadChannelRepository(ChannelsDbContext dbContext) : IThreadChannelRepository
{
    public async Task Insert(ThreadChannel threadChannel)
    {
        await dbContext.AddAsync(threadChannel);
    }
}
