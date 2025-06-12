using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.MessageChannels;
using Dapper;

namespace Babylon.Modules.Channels.Application.Messages.PinMessage;

internal sealed class PinMessageCommandHandler(IDbConnectionFactory dbConnectionFactory, 
    IMessageChannelReactionRepository messageChannelReactionRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<PinMessageCommand>
{
    public async Task<Result> Handle(PinMessageCommand request, CancellationToken cancellationToken)
    {
        await using DbConnection connetion = await dbConnectionFactory.OpenConnectionAsync();
        
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

        bool isAuthorized = await connetion.ExecuteScalarAsync<bool>(sql, new { request.ChannelId, request.Id });
        
        if (!isAuthorized)
        {
            return Result.Failure(Error.Failure(description: "User is not authorized to access this channel"));
        }
        
        const string sql2 =
            """
            CASE
              WHEN EXIST(
                 SELECT 1
                 FROM channels.message_channels mc
                 WHERE mc.id = @Id AND mc.channel_id = @ChannelId
            )
              THEN 1
              ELSE 0
            END AS ExistFlag
            """;

        int isMessage = await connetion.QuerySingleAsync<int>(sql2, new { request.ChannelId, request.Id });

        if (isMessage == 0)
        {
            return Result.Failure(Error.Failure(description: "Message was not found"));
        }

        MessageChannelReaction? reaction = await messageChannelReactionRepository.Get(request.Id, request.MessageId);
        
        if (reaction is null)
        {
            var messageReaction = MessageChannelReaction.Create(request.Id, request.MessageId, pin: request.Pin);
            await messageChannelReactionRepository.Insert(messageReaction);
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
