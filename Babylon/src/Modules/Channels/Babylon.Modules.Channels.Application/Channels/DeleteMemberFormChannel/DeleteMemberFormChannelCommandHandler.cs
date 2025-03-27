using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.Channels;

namespace Babylon.Modules.Channels.Application.Channels.DeleteMemberFormChannel;
internal sealed class DeleteMemberFormChannelCommandHandler(IChannelMemberRepository channelMemberRepository, IUnitOfWork unitOfWork) : ICommandHandler<DeleteMemberFormChannelCommand>
{
    public async Task<Result> Handle(DeleteMemberFormChannelCommand request, CancellationToken cancellationToken)
    {
        ChannelMember channelMember = await channelMemberRepository.GetChannelMember(request.ChannelId, request.MemberId);
        await channelMemberRepository.DeleteChannelMember(channelMember);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();

    }
}
