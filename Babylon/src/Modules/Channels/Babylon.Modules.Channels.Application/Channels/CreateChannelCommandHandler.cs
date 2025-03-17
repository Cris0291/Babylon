using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;

namespace Babylon.Modules.Channels.Application.Channels;
internal sealed class CreateChannelCommandHandler : ICommandHandler<CreateChannelCommand, Guid>
{
    public Task<Result<Guid>> Handle(CreateChannelCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
