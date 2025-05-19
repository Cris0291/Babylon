using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Domain.Channels;
using Dapper;

namespace Babylon.Modules.Channels.Application.Channels.ChannelArchiveValidation;
internal sealed class ChannelArchiveValidationQueryHandler(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<ChannelArchiveValidationQuery, bool>
{
    public async Task<Result<bool>> Handle(ChannelArchiveValidationQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            """
            SELECT EXIST(
               SELECT 1
               FROM channels.channels c
               WHERE c.channel_id = @ChannelId AND c.type = @Type
            )
            """;
        return await connection.ExecuteScalarAsync<bool>(sql, new { request.ChannelId, Type = ChannelType.Archived });
    }
}
