using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Application.Members.GetValidChannel;
using Babylon.Modules.Channels.Domain.MessageChannels;
using MediatR;

namespace Babylon.Modules.Channels.Application.Messages.EditMessageChannel;
internal sealed class EditMessageChannelCommandHandler(IMessageChannelRepository messageChannelRepository, ISender sender, IUnitOfWork unitOfWork) : ICommandHandler<EditMessageChannelCommand>
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

        Result<(bool, bool)> result = await sender.Send(new GetValidChannelQuery(request.Id, request.ChannelId), cancellationToken);

        if (!result.IsSuccess)
        {
            return result;
        }
        
        if (result.TValue.Item1)
        {
            return Result.Failure(Error.Failure(description: "User is mute from this channel and is not authorized to post or edit messages"));
        }

        if (result.TValue.Item2)
        {
            return Result.Failure(Error.Failure(description: "This channel is archived. New messages or reactions cannot be added"));
        }
        
        message.Edit(request.Message);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
