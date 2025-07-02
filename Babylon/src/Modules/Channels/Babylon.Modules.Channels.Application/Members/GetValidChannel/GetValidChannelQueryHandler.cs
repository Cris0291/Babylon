using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Dapper;

namespace Babylon.Modules.Channels.Application.Members.GetValidChannel;
internal sealed class GetValidChannelQueryHandler(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<GetValidChannelQuery, bool>
{
    public async Task<Result<bool>> Handle(GetValidChannelQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            """
            WITH SelectedChannel AS (
                SELECT
                    c.channel_id,
                    c.type,
                    c.blocked_members
                FROM channels.channels c
                where c.channel_id = @ChannelId
            ),
            SELECT
                sc.channel_id,
                sc.type,
                CASE WHEN sc.channel_id IS NOT NULL THEN 1 ELSE 0 END AS ExistChannel,
                CASE WHEN cm.id IS NOT NULL THEN 1 ELSE 0 END AS IsAuthorized,
                CASE WHEN cm.is_mute = 1 THEN 1 ELSE 0 END AS IsMute,
                CASE 
                  WHEN EXISTS (
                     SELECT 1
                     FROM STRING_SPLIT(sc.blocked_members, ',') split
                     WHERE SPLIT.VALUE = @Id
                  )
                  THEN 1
                  ELSE 0
                END AS IsBlocked
            FROM SelectedChannel sc
            LEFT JOIN channels.channel_members cm
                ON cm.channel_id = @ChannelId AND cm.id = @Id
            """;

        (bool isMute, bool isAuthorized, bool existChannel, bool isBlocked) = await connection.QuerySingleAsync<(bool IsMute, bool IsAuthorized, bool ExistChannel, bool IsBlocked)>(sql, new { request.ChannelId, request.Id });
        if (!isMute)
        {
            return Result.Failure(Error.Failure(description: "User must be a member of the thread"));
        }

        if (!isAuthorized)
        {
            return Result.Failure(Error.Failure(description: "Requested thread message was not found"));
        }
        if (!isBlocked)
        {
            return Result.Failure(Error.Failure(description: "Requested thread message was not found"));
        }
        if (!isAuthorized)
        {
            return Result.Failure(Error.Failure(description: "Requested thread message was not found"));
        }
    }
}
