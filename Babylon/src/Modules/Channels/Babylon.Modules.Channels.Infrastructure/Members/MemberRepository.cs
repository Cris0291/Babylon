using Babylon.Modules.Channels.Domain.Members;
using Babylon.Modules.Channels.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Babylon.Modules.Channels.Infrastructure.Members;
internal sealed class MemberRepository(ChannelsDbContext dbContext) : IMemberRepository
{
    public async Task<bool> Exist(Guid id)
    {
        Member? member = await dbContext.Members.SingleOrDefaultAsync(m => m.Id == id);
        return member is not null;
    }

    public async Task Insert(Member member)
    {
        await dbContext.AddAsync(member);
    }
    
    public async Task<bool> IsBlockedMember(Guid main, Guid participant)
    {
        Member? mainMember = await dbContext.Members
            .Include(m => m.BlockedByMembers)
            .Include(m => m.BlockedMembers)
            .SingleOrDefaultAsync(m => m.Id == main);
        
        if(mainMember == null)
        {
            throw new InvalidOperationException("Member was not found");
        }

        bool isBlocked = mainMember.IsBlockedMember(participant);

        bool isBlockedBy = mainMember.IsBlockedByMember(participant);

        if (isBlockedBy || isBlocked)
        {
            return true;
        }

        return false; 
    }
}
