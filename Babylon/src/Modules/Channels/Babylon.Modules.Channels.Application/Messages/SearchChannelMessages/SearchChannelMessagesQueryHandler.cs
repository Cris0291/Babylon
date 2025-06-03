using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Dapper;

namespace Babylon.Modules.Channels.Application.Messages.SearchChannelMessages;
internal sealed class SearchChannelMessagesQueryHandler(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<SearchChannelMessagesQuery, IEnumerable<SearchedChannelMessageDto>>
{
    public async Task<Result<IEnumerable<SearchedChannelMessageDto>>> Handle(SearchChannelMessagesQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

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
            FilteredMessages AS (
                SELECT mc.*
                FROM channels.message_channels mc
                JOIN  Authorized a ON a.channel_id = mc.channel_id
                WHERE a.IsAuthorized = 1
                   AND m.message LIKE '%' + @Search + '%'
            )
            SELECT *
            FROM FilteredMessages
            """;

        IEnumerable<SearchedChannelMessageDto> messages = await connection.QueryAsync<SearchedChannelMessageDto>(sql, new { request.ChannelId, request.Id, request.Search});
    }
}
