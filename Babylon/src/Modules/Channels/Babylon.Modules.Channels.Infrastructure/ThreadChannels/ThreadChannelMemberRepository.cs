using Babylon.Modules.Channels.Domain.ThreadChannels;
using Babylon.Modules.Channels.Infrastructure.Database;

namespace Babylon.Modules.Channels.Infrastructure.ThreadChannels;
internal sealed class ThreadChannelMemberRepository(ChannelsDbContext dbContext) : IThreadChannelMemberRepository
{
    public async Task Insert(IEnumerable<ThreadChannelMember> members)
    {
        await dbContext.AddRangeAsync(members);
    }
}
