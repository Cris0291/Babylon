using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.MessageThreadChannels;
using Dapper;

namespace Babylon.Modules.Channels.Application.ThreadMessages.PinThreadMessage;

internal sealed class PinThreadMessageCommandHandler(IDbConnectionFactory dbConnectionFactory, IMessageThreadChannelReactionRepository reactionRepository, IUnitOfWork unitOfWork) : ICommandHandler<PinThreadMessageCommand>
{
    public async Task<Result> Handle(PinThreadMessageCommand request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();
        
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
        (bool isMember, bool existThreadMessage) = await connection.QuerySingleAsync<(bool IsMemeber, bool ExistThreadMessage)>(sql, new { request.ThreadChannelId, request.Id, request.ThreadMessageId });
        if (!isMember)
        {
            return Result.Failure<int>(Error.Failure(description: "User must be a member of the thread"));
        }

        if (!existThreadMessage)
        {
            return Result.Failure<int>(Error.Failure(description: "Requested thread message was not found"));
        }
        
        MessageThreadChannelReaction? reaction = await reactionRepository.Get(request.Id, request.ThreadMessageId);

        if (reaction is null)
        {
            var messageReaction = MessageThreadChannelReaction.Create(request.Id, request.ThreadMessageId, pin: request.Pin);
            await reactionRepository.Insert(messageReaction);
        }
        else
        {
            Result result = reaction.AddOrRemovePin(request.Pin);
            if (result.IsSuccess)
            {
                return result;
            }
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
