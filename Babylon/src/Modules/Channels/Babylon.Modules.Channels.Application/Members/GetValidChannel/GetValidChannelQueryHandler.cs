using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Channels.GetChannelStateAccess;
using Dapper;

namespace Babylon.Modules.Channels.Application.Members.GetValidChannel;
internal sealed class GetValidChannelQueryHandler(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<GetValidChannelQuery, ChannelAccessStateDto>
{
    public async Task<Result<ChannelAccessStateDto>> Handle(GetValidChannelQuery request, CancellationToken cancellationToken)
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
                cm.is_mute AS IsMute,
                CASE WHEN sc.channel_id IS NOT NULL THEN 1 ELSE 0 END AS ExistChannel,
                CASE WHEN sc.type = 'Archive' THEN 1 ELSE 0 AS IsArchived
                CASE
              WHEN c.type = 'Public'       THEN 1
              WHEN c.a_type = 'ArchivePublic' THEN 1
              WHEN cm.id   IS NOT NULL     THEN 1
              ELSE 0
            END AS IsAuthorized,
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

        (bool isAuthorized, bool existChannel, bool isBlocked, bool isMute, bool isArchived) = await connection.QuerySingleAsync<(bool IsAuthorized, bool ExistChannel, bool IsBlocked, bool IsMute, bool IsArchived)>(sql, new { request.ChannelId, request.Id });

        return Result.Success(new ChannelAccessStateDto(isArchived, isBlocked, isMute, existChannel, isAuthorized, false));
    }
}
