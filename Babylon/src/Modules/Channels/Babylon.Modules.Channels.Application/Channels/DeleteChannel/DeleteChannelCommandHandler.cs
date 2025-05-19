using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.Channels;

namespace Babylon.Modules.Channels.Application.Channels.DeleteChannel;
internal sealed class DeleteChannelCommandHandler(IChannelRepository channelRepository, IUnitOfWork unitOfWork) : ICommandHandler<DeleteChannelCommand>
{
    public async Task<Result> Handle(DeleteChannelCommand request, CancellationToken cancellationToken)
    {
        Channel? channel = await channelRepository.GetChannel(request.ChannelId);

        if (channel is null)
        {
            throw new InvalidOperationException("Channel was not found");
        }

        await channelRepository.Delete(channel);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
