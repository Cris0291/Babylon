using Babylon.Modules.Channels.Domain.Channels;
using Babylon.Modules.Channels.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Babylon.Modules.Channels.Infrastructure.Channels;
internal class ChannelMemberRepository(ChannelsDbContext dbContext) : IChannelMemberRepository
{
    public async Task AddChannelMembers(IEnumerable<ChannelMember> members)
    {
        await dbContext.ChannelMembers.AddRangeAsync(members);
    }
    public async Task DeleteChannelMember(ChannelMember channelMember)
    {
        dbContext.Remove(channelMember);
        await Task.CompletedTask;
    }
    public async Task<ChannelMember> GetChannelMember(Guid channelId, Guid memberId)
    {
        ChannelMember? channelMember = await dbContext.ChannelMembers.SingleOrDefaultAsync(c => c.ChannelId == channelId && c.MemberId == memberId);

        if (channelMember is null)
        {
            throw new InvalidOperationException("Requested channel memeber was not found");
        }

        return channelMember;
    }
}
