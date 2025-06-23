using Babylon.Modules.Channels.Domain.MessageThreadChannels;
using Babylon.Modules.Channels.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Babylon.Modules.Channels.Infrastructure.MessageThreadChannels;

internal sealed class MessageThreadChannelReactionRepository(ChannelsDbContext dbContext) :IMessageThreadChannelReactionRepository
{
    public async Task<MessageThreadChannelReaction?> Get(Guid messageThreadChannelId, Guid id)
    {
        return await dbContext.MessageThreadChannelReactions.SingleOrDefaultAsync(x => x.MessageThreadChannelId == messageThreadChannelId && x.Id == id);
    }

    public async Task Insert(MessageThreadChannelReaction reaction)
    {
        await dbContext.AddAsync(reaction);
    }
}
