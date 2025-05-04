using Babylon.Modules.Channels.Domain.MessageChannels;
using Babylon.Modules.Channels.Infrastructure.Database;

namespace Babylon.Modules.Channels.Infrastructure.MessageChannels;
internal sealed class MessageChannelRepository(ChannelsDbContext dbContext) : IMessageChannelRepository
{
    public async Task Insert(MessageChannel messageChannel)
    {
        await dbContext.AddAsync(messageChannel);
    }
}
