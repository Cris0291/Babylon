using Babylon.Modules.Channels.Domain.MessageChannels;
using Babylon.Modules.Channels.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Babylon.Modules.Channels.Infrastructure.MessageChannels;
internal sealed class MessageChannelRepository(ChannelsDbContext dbContext) : IMessageChannelRepository
{
    public async Task<MessageChannel?> Get(Guid messageChannelId)
    {
        return await dbContext.MesssageChannels.SingleOrDefaultAsync(x => x.MessageChannelId == messageChannelId);
    }

    public async Task Insert(MessageChannel messageChannel)
    {
        await dbContext.AddAsync(messageChannel);
    }
}
