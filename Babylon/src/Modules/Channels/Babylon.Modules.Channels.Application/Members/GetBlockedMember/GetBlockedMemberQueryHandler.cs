using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Domain.Members;

namespace Babylon.Modules.Channels.Application.Members.GetBlockedMember;

internal sealed class GetBlockedMemberQueryHandler(IMemberRepository memberRepository) : IQueryHandler<GetBlockedMemberQuery, bool>
{
    public async Task<Result<bool>> Handle(GetBlockedMemberQuery request, CancellationToken cancellationToken)
    {
        bool isBlocked = false;
        
        try
        {
            isBlocked = await memberRepository.IsBlockedMember(request.MainMember, request.ParticipantMember);
        }
        catch (Exception e)
        {
            return Result.Failure<bool>(Error.NotFound(description: e.Message));
        }

        return Result.Success(isBlocked);
    }
}
