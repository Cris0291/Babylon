using Babylon.Modules.Channels.Domain.Channels;
using Babylon.Modules.Channels.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Babylon.Modules.Channels.Infrastructure.Channels;
internal class ChannelMemberRepository(ChannelsDbContext dbContext) : IChannelMemberRepository
{
    public async Task AddChannelMembers(ChannelMember member)
    {
        await dbContext.ChannelMembers.AddAsync(member);
    }
    public async Task DeleteChannelMember(ChannelMember channelMember)
    {
        dbContext.Remove(channelMember);
        await Task.CompletedTask;
    }
    public async Task<ChannelMember?> GetChannelMember(Guid channelId, Guid id)
    {
        return await dbContext.ChannelMembers.SingleOrDefaultAsync(c => c.ChannelId == channelId && c.Id == id);
    }
}
