using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Domain.MessageChannels;
using Dapper;

namespace Babylon.Modules.Channels.Application.Messages.PinMessage;

internal sealed class PinMessageCommandHandler(IDbConnectionFactory dbConnectionFactory, IMessageChannelReactionRepository messageChannelReactionRepository) : ICommandHandler<PinMessageCommand>
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
            return Result.Failure<int>(Error.Failure(description: "User is not authorized to access this channel"));
        }
    }
}
