using Babylon.Modules.Channels.Domain.MessageThreadChannels;
using Babylon.Modules.Channels.Infrastructure.Database;

namespace Babylon.Modules.Channels.Infrastructure.MessageThreadChannels;
internal sealed class MessageThreadChannelRepository(ChannelsDbContext dbContext) : IMessageThreadChannelRepository
{
    public async Task Insert(MessageThreadChannel messageThreadChannel)
    {
        await dbContext.AddAsync(messageThreadChannel);
    }
}
