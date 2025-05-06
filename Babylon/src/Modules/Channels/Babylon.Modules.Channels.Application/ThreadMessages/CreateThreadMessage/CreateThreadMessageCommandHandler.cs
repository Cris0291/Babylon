using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;

namespace Babylon.Modules.Channels.Application.ThreadMessages.CreateThreadMessage;
internal sealed class CreateThreadMessageCommandHandler : ICommandHandler<CreateThreadMessageCommand>
{
    public async Task<Result> Handle(CreateThreadMessageCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
