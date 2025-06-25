using Babylon.Modules.Channels.Domain.MessageThreadChannels;
using Babylon.Modules.Channels.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Babylon.Modules.Channels.Infrastructure.MessageThreadChannels;
internal sealed class MessageThreadChannelRepository(ChannelsDbContext dbContext) : IMessageThreadChannelRepository
{
    public async Task Insert(MessageThreadChannel messageThreadChannel)
    {
        await dbContext.AddAsync(messageThreadChannel);
    }

    public async Task<MessageThreadChannel?> Get(Guid messageThreadChannelId)
    {
        return await dbContext.MessageThreadChannels.SingleOrDefaultAsync(x => x.MessageThreadChannelId == messageThreadChannelId);
    }
}
