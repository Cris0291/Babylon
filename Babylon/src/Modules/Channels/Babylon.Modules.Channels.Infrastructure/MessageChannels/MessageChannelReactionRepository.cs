using Babylon.Modules.Channels.Domain.MessageChannels;
using Babylon.Modules.Channels.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Babylon.Modules.Channels.Infrastructure.MessageChannels;
internal sealed class MessageChannelReactionRepository(ChannelsDbContext dbContext) : IMessageChannelReactionRepository
{
    public async Task<MessageChannelReaction?> Get(Guid id, Guid messageChannelId)
    {
        return await dbContext.MessageChannelReactions.SingleOrDefaultAsync(x => x.Id == id && x.MessageChannelId == messageChannelId);
    }

    public async Task Insert(MessageChannelReaction reaction)
    {
        await dbContext.AddAsync(reaction);
    }
}
