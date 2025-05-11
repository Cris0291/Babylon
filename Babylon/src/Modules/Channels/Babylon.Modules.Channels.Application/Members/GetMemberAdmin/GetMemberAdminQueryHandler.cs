using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Dapper;

namespace Babylon.Modules.Channels.Application.Members.GetMemberAdmin;
internal sealed class GetMemberAdminQueryHandler(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<GetMemberAdminQuery, bool>
{
    public async Task<Result<bool>> Handle(GetMemberAdminQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            """
            SELECT EXIST(
               SELECT 1
               FROM channels.channels c
               WHERE c.creator = @Id AND c.channel_id = @ChannelId
            )
            """;

        return await connection.ExecuteScalarAsync<bool>(sql, request);
    }
}
