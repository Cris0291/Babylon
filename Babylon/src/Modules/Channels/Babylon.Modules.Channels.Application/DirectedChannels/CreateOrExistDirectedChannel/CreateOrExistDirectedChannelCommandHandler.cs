using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;

namespace Babylon.Modules.Channels.Application.DirectedChannels.CreateOrExistDirectedChannel;

internal sealed class CreateOrExistDirectedChannelCommandHandler : ICommandHandler<CreateOrExistDirectedChannelCommand, Guid>
{
    public Task<Result<Guid>> Handle(CreateOrExistDirectedChannelCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
