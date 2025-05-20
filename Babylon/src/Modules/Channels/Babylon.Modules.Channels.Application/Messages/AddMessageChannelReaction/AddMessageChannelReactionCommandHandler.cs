using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.MessageChannels;

namespace Babylon.Modules.Channels.Application.Messages.AddMessageChannelReaction;
internal sealed class AddMessageChannelReactionCommandHandler(IMessageChannelRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<AddMessageChannelReactionCommand>
{
    public async Task<Result> Handle(AddMessageChannelReactionCommand request, CancellationToken cancellationToken)
    {
        MessageChannel? message = await repository.Get(request.MessageId);

        if (message is null)
        {
            throw new InvalidOperationException("Message was not found");
        }


    }
}
