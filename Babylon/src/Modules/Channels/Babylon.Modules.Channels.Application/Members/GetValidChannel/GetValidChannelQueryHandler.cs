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
            MemberInfo AS (
                 SELECT
                     cm.id,
                     cm.is_mute
                 FROM channels.channel_members cm
                 WHERE cm.channel_id = @ChannelId AND cm.id = @Id
            )
            SELECT
                sc.channel_id,
                sc.type,
                CASE WHEN sc.channel_id IS NOT NULL THEN 1 ELSE 0 END AS ExistChannel,
                CASE WHEN mi.id IS NOT NULL THEN 1 ELSE 0 END AS IsAuthorized,
                CASE WHEN mi.is_mute = 1 THEN 1 ELSE 0 END AS IsMute,
            FROM SelectedChannel sc
            """;

        bool isAuthorized = await connection.ExecuteScalarAsync<int>(sql, new { request.Id, request.ChannelId }) == 1;

        if (!isAuthorized)
        {
            return Result.Failure<bool>(Error.Failure(description: "User is not authorized to access this channel"));
        }
    }
}
