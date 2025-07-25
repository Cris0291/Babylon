using Babylon.Modules.Channels.Domain.DirectedChannels;
using Babylon.Modules.Channels.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Babylon.Modules.Channels.Infrastructure.DirectedChannels;

internal sealed class DirectedChannelRepository(ChannelsDbContext dbContext) : IDirectedChannelRepository
{
    public async Task<DirectedChannel?> Get(Guid creator)
    {
        await dbContext.DirectedChannels.SingleOrDefaultAsync(x => x.);
    }
}
