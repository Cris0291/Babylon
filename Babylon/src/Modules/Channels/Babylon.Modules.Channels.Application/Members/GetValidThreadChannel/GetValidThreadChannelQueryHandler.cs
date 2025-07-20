using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Channels.GetChannelStateAccess;
using Dapper;

namespace Babylon.Modules.Channels.Application.Members.GetValidThreadChannel;
internal sealed class GetValidThreadChannelQueryHandler(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<GetValidThreadChannelQuery, ChannelAccessStateDto>
{
    public async Task<Result<ChannelAccessStateDto>> Handle(GetValidThreadChannelQuery request, CancellationToken cancellationToken)
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
                CASE WHEN tc.thread_channel_id IS NOT NULL THEN 1 ELSE 0 S ExistThread,
                CASE WHEN sc.channel_id IS NOT NULL THEN 1 ELSE 0 END AS ExistChannel,
                CASE WHEN sc.type = 'Archive' THEN 1 ELSE 0 AS IsArchived
                CASE WHEN cm.id  IS NOT NULL  THEN 1 ELSE 0 END AS IsAuthorized,
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
            LEFT JOIN channels.thread_channels tc
                ON tc.channel_id = @ChannelId AND tc.thread_channel_id = @ThreadChannelId
            """;

        (bool isAuthorized, bool existChannel, bool isBlocked, bool isMute, bool isArchived, bool existThread) = await connection.QuerySingleAsync<(bool IsAuthorized, bool ExistChannel, bool IsBlocked, bool IsMute, bool IsArchived, bool ExistThread)>(sql, new { request.ChannelId, request.Id, request.ThreadChannelId });

        return Result.Success(new ChannelAccessStateDto(isArchived, isBlocked, isMute, existChannel, isAuthorized, existThread));
    }
}
