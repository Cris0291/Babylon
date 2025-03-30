using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.Channels;

namespace Babylon.Modules.Channels.Application.Channels.ChangeChannelType;
internal sealed class ChangeChannelTypeCommandHandler(IChannelRepository channelRepository, IUnitOfWork unitOfWork) : ICommandHandler<ChangeChannelTypeCommand>
{
    public async Task<Result> Handle(ChangeChannelTypeCommand request, CancellationToken cancellationToken)
    {
        Channel? channel = await channelRepository.GetChannel(request.ChannelId);
        if(channel is null)
        {
            throw new InvalidOperationException("Channel was not found");
        }

        if(channel.Creator != request.ChannelCreator)
        {
            throw new InvalidOperationException("Only channel creator can modify channel type");
        }

        Result result = channel.ChangeType(request.ChannelType);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }
}
