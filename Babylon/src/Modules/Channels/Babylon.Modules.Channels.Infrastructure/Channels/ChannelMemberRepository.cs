using Babylon.Modules.Channels.Domain.Channels;
using Babylon.Modules.Channels.Infrastructure.Database;

namespace Babylon.Modules.Channels.Infrastructure.Channels;
internal class ChannelMemberRepository(ChannelsDbContext dbContext) : IChannelMemberRepository
{
    public async Task AddChannelMembers(IEnumerable<ChannelMember> members)
    {
        await dbContext.ChannelMembers.AddRangeAsync(members);
    }
}
