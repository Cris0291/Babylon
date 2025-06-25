using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.MessageThreadChannels;
using Dapper;

namespace Babylon.Modules.Channels.Application.ThreadMessages.AddOrRemoveMessageThreadChannelLike;

internal sealed class AddOrRemoveMessageThreadChannelLikeCommandHandler(IDbConnectionFactory dbConnectionFactory, 
    IMessageThreadChannelReactionRepository reactionRepository, 
    IUnitOfWork unitOfWork,
    IMessageThreadChannelRepository messageThreadChannelRepository) : ICommandHandler<AddOrRemoveMessageThreadChannelLikeCommand, int>
{
    public async Task<Result<int>> Handle(AddOrRemoveMessageThreadChannelLikeCommand request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();
        
        MessageThreadChannel? message = await messageThreadChannelRepository.Get(request.ThreadChannelMessageId);

        if (message is null)
        {
            return Result.Failure<int>(Error.Failure(description: "Message was not found"));
        }
        
        const string sql = 
            """
            SELECT CAST(
               CASE
                 WHEN EXISTS(
                    SELECT 1
                    FROM channels.thread_channel_members tcm
                    WHERE tcm.thread_channel_id = @ThreadChannelId AND tcm.id = @Id 
                 )
                   THEN 1
                   ELSE 0
                END AS bit
            ) AS IsMemeber,
                CAST(
                  CASE 
                    WHEN EXISTS(
                        SELECT 1 
                        FROM channels.message_thread_channels mtc
                        WHERE mtc.message_thread_channel_id = @ThreadChannelMessageId
                    )
                THEN 1
                ELSE 0
                END AS bit
                ) AS ExistThreadMessage
            """;
        (bool isMember, bool existThreadMessage) = await connection.QuerySingleAsync<(bool IsMemeber, bool ExistThreadMessage)>(sql, new { request.ThreadChannelId, request.Id, request.ThreadChannelMessageId });
        if (!isMember)
        {
            return Result.Failure<int>(Error.Failure(description: "User must be a member of the thread"));
        }

        if (!existThreadMessage)
        {
            return Result.Failure<int>(Error.Failure(description: "Requested thread message was not found"));
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
