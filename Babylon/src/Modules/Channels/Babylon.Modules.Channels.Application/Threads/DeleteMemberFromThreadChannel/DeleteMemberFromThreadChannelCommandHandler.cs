using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.ThreadChannels;
using Dapper;

namespace Babylon.Modules.Channels.Application.Threads.DeleteMemberFromThreadChannel;

internal sealed class DeleteMemberFromThreadChannelCommandHandler(IThreadChannelMemberRepository threadChannelMemberRepository, IUnitOfWork unitOfWork) : ICommandHandler<DeleteMemberFromThreadChannelCommand>
{
    public async Task<Result> Handle(DeleteMemberFromThreadChannelCommand request, CancellationToken cancellationToken)
    {
        ThreadChannelMember? member = await threadChannelMemberRepository.Get(request.Id, request.ThreadChannelId);
        
        if(member == null)
        {
            return Result.Failure(Error.Failure(description: "Member was not part of the channel"));
        }

        await threadChannelMemberRepository.DeleteThreadChannelMember(member);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
