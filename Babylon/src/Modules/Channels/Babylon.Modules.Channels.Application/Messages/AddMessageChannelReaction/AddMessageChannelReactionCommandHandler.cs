using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Application.Members.GetValidChannel;
using Babylon.Modules.Channels.Domain.MessageChannels;
using Dapper;
using MediatR;

namespace Babylon.Modules.Channels.Application.Messages.AddMessageChannelReaction;
internal sealed class AddMessageChannelReactionCommandHandler(
    IMessageChannelRepository repository, 
    IMessageChannelReactionRepository messageChannelReactionRepository ,
    ISender sender,
    IUnitOfWork unitOfWork) : ICommandHandler<AddMessageChannelReactionCommand>
{
    public async Task<Result> Handle(AddMessageChannelReactionCommand request, CancellationToken cancellationToken)
    {
        MessageChannel? message = await repository.Get(request.MessageId);

        if (message is null)
        {
            return Result.Failure(Error.Failure(description: "Message was not found"));
        }

        Result<(bool, bool)> result = await sender.Send(new GetValidChannelQuery(request.Id, request.ChannelId), cancellationToken);

        if (!result.IsSuccess)
        {
            return result;
        }

        if (result.TValue.Item2)
        {
            return Result.Failure(Error.Failure(description: "This channel is archived. New messages or reactions cannot be added"));
        }

        MessageChannelReaction? reaction = await messageChannelReactionRepository.Get(request.Id, request.MessageId);

        if (reaction is null)
        {
            var messageReaction = MessageChannelReaction.Create(request.Id, request.MessageId, request.Emoji);
            await messageChannelReactionRepository.Insert(messageReaction);
        }
        else
        {
            reaction.AddOrToggleEmoji(request.Emoji);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
