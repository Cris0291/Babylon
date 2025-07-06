using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Dapper;

namespace Babylon.Modules.Channels.Application.Channels.GetChannelStateAccess;
internal sealed class GetChannelStateAccessQueryHandler(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<GetChannelStateAccessQuery, ChannelAccessStateDto>
{
    public async Task<Result<ChannelAccessStateDto>> Handle(GetChannelStateAccessQuery request, CancellationToken cancellationToken)
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
                cm.is_mute AS IsMute,
                sc.type AS Type
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

        ChannelAccessStateDto channelState = await connection.QuerySingleAsync<ChannelAccessStateDto>(sql, request);

        return Result.Success(channelState);
    }
}
