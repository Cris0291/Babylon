using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.Channels;
using Babylon.Modules.Channels.Domain.Members;

namespace Babylon.Modules.Channels.Application.Channels.DeleteMemberFormChannel;
internal sealed class DeleteMemberFormChannelCommandHandler(
    IChannelMemberRepository channelMemberRepository, 
    IUnitOfWork unitOfWork,
    IChannelRepository channelRepository) : ICommandHandler<DeleteMemberFormChannelCommand>
{
    public async Task<Result> Handle(DeleteMemberFormChannelCommand request, CancellationToken cancellationToken)
    {
        Channel? channel = await channelRepository.GetChannel(request.ChannelId);
        if (channel is null)
        {
            throw new InvalidOperationException("Channel was not found");
        }

        ChannelMember? channelMember = await channelMemberRepository.GetChannelMember(request.ChannelId, request.Id);

        if(channelMember == null)
        {
            throw new InvalidOperationException("Member was not part of the channel");
        }

        await channelMemberRepository.DeleteChannelMember(channelMember);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();

    }
}
