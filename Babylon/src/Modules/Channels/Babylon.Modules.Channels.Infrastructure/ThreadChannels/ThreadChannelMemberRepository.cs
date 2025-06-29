using Babylon.Modules.Channels.Domain.ThreadChannels;
using Babylon.Modules.Channels.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Babylon.Modules.Channels.Infrastructure.ThreadChannels;
internal sealed class ThreadChannelMemberRepository(ChannelsDbContext dbContext) : IThreadChannelMemberRepository
{
    public async Task Insert(IEnumerable<ThreadChannelMember> members)
    {
        await dbContext.AddRangeAsync(members);
    }
    public async Task<ThreadChannelMember?> Get(Guid id , Guid threadChannelId)
    {
        return await dbContext.ThreadChannelMembers.SingleOrDefaultAsync(m => m.Id == id && m.ThreadChannelId == threadChannelId);
    }
    public async Task DeleteThreadChannelMember(ThreadChannelMember member)
    {
        dbContext.Remove(member);
        await Task.CompletedTask;
    }
}
