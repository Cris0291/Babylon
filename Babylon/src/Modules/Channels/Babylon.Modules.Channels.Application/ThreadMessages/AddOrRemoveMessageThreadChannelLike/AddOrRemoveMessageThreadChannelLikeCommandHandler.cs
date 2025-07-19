using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.MessageThreadChannels;
using Dapper;

namespace Babylon.Modules.Channels.Application.ThreadMessages.AddOrRemoveMessageThreadChannelLike;

internal sealed class AddOrRemoveMessageThreadChannelLikeCommandHandler( 
    IMessageThreadChannelReactionRepository reactionRepository, 
    IUnitOfWork unitOfWork,
    IMessageThreadChannelRepository messageThreadChannelRepository) : ICommandHandler<AddOrRemoveMessageThreadChannelLikeCommand, int>
{
    public async Task<Result<int>> Handle(AddOrRemoveMessageThreadChannelLikeCommand request, CancellationToken cancellationToken)
    {
        MessageThreadChannel? message = await messageThreadChannelRepository.Get(request.ThreadChannelMessageId);

        if (message is null)
        {
            return Result.Failure<int>(Error.Failure(description: "Message was not found"));
        }
        
        MessageThreadChannelReaction? reaction = await reactionRepository.Get(request.Id, request.ThreadChannelMessageId);

        if (reaction is null)
        {
            var messageReaction = MessageThreadChannelReaction.Create(request.Id, request.ThreadChannelMessageId, like: request.Like);
            await reactionRepository.Insert(messageReaction);
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
