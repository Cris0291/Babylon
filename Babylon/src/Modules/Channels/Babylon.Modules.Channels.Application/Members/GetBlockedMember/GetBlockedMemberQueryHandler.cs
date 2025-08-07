using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;

namespace Babylon.Modules.Channels.Application.Members.GetBlockedMember;

internal sealed class GetBlockedMemberQueryHandler : IQueryHandler<GetBlockedMemberQuery, bool>
{
    public async Task<Result<bool>> Handle(GetBlockedMemberQuery request, CancellationToken cancellationToken)
    {
        l
    }
}
