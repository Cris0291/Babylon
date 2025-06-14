using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Dapper;

namespace Babylon.Modules.Channels.Application.Members.GetValidThreadChannel;
internal sealed class GetValidThreadChannelQueryHandler(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<GetValidThreadChannelQuery, bool>
{
    public async Task<Result<bool>> Handle(GetValidThreadChannelQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            """
            CASE
             WHEN EXIST(
               SELECT 1
               FROM channels.thread_channel_member tm
               WHERE tm.id = @Id AND tm.thread_channel_id = @ThreadId
            )
             THEN 1
             ELSE 0
            END AS ExistFlag
            """;

        return await connection.QuerySingleAsync<int>(sql, request) == 1 ? true : false;
    }
}
