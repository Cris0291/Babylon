using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.Channels;

namespace Babylon.Modules.Channels.Application.Channels.BlockMemberFromChannel;
internal sealed class BlockMemberFromChannelCommandHandler(
    IChannelRepository channelRepository, 
    IChannelMemberRepository channelMemberRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<BlockMemberFromChannelCommand>
{
    public async Task<Result> Handle(BlockMemberFromChannelCommand request, CancellationToken cancellationToken)
    {
        Channel? channel = await channelRepository.GetChannel(request.ChannelId);
        if (channel is null)
        {
            throw new InvalidOperationException("Channel was not found");
        }

        ChannelMember channelMemeber = await channelMemberRepository.GetChannelMember(request.ChannelId, request.Id);

        channel.BlockMember(request.Id);

        await channelMemberRepository.DeleteChannelMember(channelMemeber);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
