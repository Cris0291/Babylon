using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.MessageChannels;
using Dapper;

namespace Babylon.Modules.Channels.Application.Messages.AddOrRemoveMessageChannelLike;

internal sealed class AddOrRemoveMessageChannelLikeCommandHandler(
    IMessageChannelRepository repository, 
    IMessageChannelReactionRepository messageChannelReactionRepository,
    IDbConnectionFactory dbConnectionFactory,
    IUnitOfWork unitOfWork) : ICommandHandler<AddOrRemoveMessageChannelLikeCommand, int>
{
    public async Task<Result<int>> Handle(AddOrRemoveMessageChannelLikeCommand request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();
        
        MessageChannel? message = await repository.Get(request.MessageId);

        if (message is null)
        {
            return Result.Failure<int>(Error.Failure(description: "Message was not found"));
        }

        const string sql =
            """
            WITH SelectedChannel AS (
                SELECT channel_id, type
                FROM channels.channels
                WHERE channel_id = @ChannelId
            ),
            Authorized AS (
                SELECT
                   sc.channel_id,
                   sc.type,
                   CASE
                       WHEN sc.type = 'Public' THEN 1
                       WHEN EXISTS (
                           SELECT 1
                           FROM channels.channel_members CM
                           WHERE cm.channel_id = @ChannelId AND cm.id = @Id
                       ) THEN 1
                       ELSE 0
                   END AS IsAuthorized
                FROM SelectedChannel sc
            ),
            SELECT IsAuthorized
            FROM Authorized
            """;

        bool isAuthorized = await connection.ExecuteScalarAsync<int>(sql, new { request.Id, request.ChannelId }) == 1;

        if (!isAuthorized)
        {
            return Result.Failure<int>(Error.Failure(description: "User is not authorized to access this channel"));
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
