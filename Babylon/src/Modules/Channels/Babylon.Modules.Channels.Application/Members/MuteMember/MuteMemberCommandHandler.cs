using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.Members;

namespace Babylon.Modules.Channels.Application.Members.MuteMember;
internal sealed class MuteMemberCommandHandler(IMemberRepository memberRepository, IUnitOfWork unitOfWork) : ICommandHandler<MuteMemberCommand>
{
    public async Task<Result> Handle(MuteMemberCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
