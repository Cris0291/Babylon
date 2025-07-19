using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Application.Members.GetValidChannel;
using Babylon.Modules.Channels.Domain.MessageChannels;
using Dapper;
using MediatR;

namespace Babylon.Modules.Channels.Application.Messages.AddOrRemoveMessageChannelLike;

internal sealed class AddOrRemoveMessageChannelLikeCommandHandler(
    IMessageChannelRepository repository, 
    IMessageChannelReactionRepository messageChannelReactionRepository,
    ISender sender,
    IUnitOfWork unitOfWork) : ICommandHandler<AddOrRemoveMessageChannelLikeCommand, int>
{
    public async Task<Result<int>> Handle(AddOrRemoveMessageChannelLikeCommand request, CancellationToken cancellationToken)
    {
        MessageChannel? message = await repository.Get(request.MessageId);

        if (message is null)
        {
            return Result.Failure<int>(Error.Failure(description: "Message was not found"));
        }

        Result<(bool, bool)> res = await sender.Send(new GetValidChannelQuery(request.Id, request.ChannelId), cancellationToken);

        if (!res.IsSuccess)
        {
            return Result.Failure<int>(Error.Failure(description: res.Error!.Description));
        }

        if (res.TValue.Item2)
        {
            return Result.Failure<int>(Error.Failure(description: "This channel is archived. New messages or reactions cannot be added"));
        }
        
        MessageChannelReaction? reaction = await messageChannelReactionRepository.Get(request.Id, request.MessageId);

        if (reaction is null)
        {
            var messageReaction = MessageChannelReaction.Create(request.Id, request.MessageId, like: request.Like);
            await messageChannelReactionRepository.Insert(messageReaction);
        }
        else
        {
            Result<int> result = reaction.AddOrRemoveLike(request.Like);
            if (!result.IsSuccess)
            {
                return result;
            }
        }

        int numberOfLikes = message.AddOrRemoveLike(request.Like);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(numberOfLikes);
    }
}
