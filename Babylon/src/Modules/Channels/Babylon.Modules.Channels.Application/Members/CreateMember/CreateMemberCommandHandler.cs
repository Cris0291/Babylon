using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.Members;

namespace Babylon.Modules.Channels.Application.Members.CreateMember;
internal sealed class CreateMemberCommandHandler(IMemberRepository memberRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateMemberCommand>
{
    public async Task<Result> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        var member = Member.Create(request.UserId, request.Email, request.FirstName, request.LastName);

        await memberRepository.Insert(member);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
