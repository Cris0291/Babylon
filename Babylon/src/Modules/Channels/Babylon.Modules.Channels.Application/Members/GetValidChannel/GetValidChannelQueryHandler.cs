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
            SELECT EXIST(
               SELECT 1
               FROM channels.channel_members cm
               WHERE cm.id = @MemberId AND cm.channel_id = @ChannelId
            )
            """;

        return await connection.ExecuteScalarAsync<bool>(sql, request);
    }
}
