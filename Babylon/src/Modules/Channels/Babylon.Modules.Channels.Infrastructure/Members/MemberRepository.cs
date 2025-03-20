using Babylon.Modules.Channels.Domain.Members;
using Babylon.Modules.Channels.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Babylon.Modules.Channels.Infrastructure.Members;
internal sealed class MemberRepository(ChannelsDbContext dbContext) : IMemberRepository
{
    public async Task<bool> Exist(Guid MemberId)
    {
        Member? member = await dbContext.Members.SingleOrDefaultAsync(m => m.MemberId == MemberId);
        return member is not null;
    }
}
