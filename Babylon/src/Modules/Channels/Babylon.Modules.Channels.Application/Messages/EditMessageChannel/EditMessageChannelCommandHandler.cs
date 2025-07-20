using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Application.Members.GetValidChannel;
using Babylon.Modules.Channels.Domain.MessageChannels;
using MediatR;

namespace Babylon.Modules.Channels.Application.Messages.EditMessageChannel;
internal sealed class EditMessageChannelCommandHandler(IMessageChannelRepository messageChannelRepository, IUnitOfWork unitOfWork) : ICommandHandler<EditMessageChannelCommand>
{
    public async Task<Result> Handle(EditMessageChannelCommand request, CancellationToken cancellationToken)
    {
        MessageChannel? message = await messageChannelRepository.Get(request.MessageChannelId);
        
        if(message == null)
        {
            return Result.Failure(Error.Failure(description: "Requested message was not found"));
        }
        
        if(!message.IsMessageFromUser(request.Id))
        {
            return Result.Failure(Error.Failure(description: "User can only edit its own messages"));
        }
        
        message.Edit(request.Message);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
