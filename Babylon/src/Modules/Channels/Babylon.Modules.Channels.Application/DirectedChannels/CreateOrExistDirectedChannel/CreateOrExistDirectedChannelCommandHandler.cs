using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.DirectedChannels;

namespace Babylon.Modules.Channels.Application.DirectedChannels.CreateOrExistDirectedChannel;

internal sealed class CreateOrExistDirectedChannelCommandHandler(IDirectedChannelRepository directedChannelRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateOrExistDirectedChannelCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateOrExistDirectedChannelCommand request, CancellationToken cancellationToken)
    {
        DirectedChannel? directedChannel;
        directedChannel = await directedChannelRepository.Get(request.Creator, request.Participant);
        if(directedChannel != null)
        {
            return directedChannel.DirectedChannelId;
        }

        directedChannel = DirectedChannel.Create(request.Creator, request.Participant);
        await directedChannelRepository.Create(directedChannel);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return directedChannel.DirectedChannelId;
    }
}
