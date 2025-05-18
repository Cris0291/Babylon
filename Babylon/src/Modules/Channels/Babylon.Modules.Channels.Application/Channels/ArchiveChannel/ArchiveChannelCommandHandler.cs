using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.Channels;

namespace Babylon.Modules.Channels.Application.Channels.ArchiveChannel;
internal sealed class ArchiveChannelCommandHandler(IUnitOfWork unitOfWork, IChannelRepository channelRepository) : ICommandHandler<ArchiveChannelCommand>
{
    public async Task<Result> Handle(ArchiveChannelCommand request, CancellationToken cancellationToken)
    {
        Channel? channel = await channelRepository.GetChannel(request.ChannelId);

        if (channel is null)
        {
            throw new InvalidOperationException("Channel was not found");
        }

        channel.ArchiveChannel();

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
