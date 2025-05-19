using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.Channels;

namespace Babylon.Modules.Channels.Application.Channels.RenameChannel;
internal sealed class RenameChannelCommandHandler(IChannelRepository channelRepository, IUnitOfWork unitOfWork) : ICommandHandler<RenameChannelCommand>
{
    public async Task<Result> Handle(RenameChannelCommand request, CancellationToken cancellationToken)
    {
        Channel? channel = await channelRepository.GetChannel(request.ChannelId);

        if (channel is null)
        {
            throw new InvalidOperationException("Channel was not found");
        }

        channel.Rename(request.Name);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
