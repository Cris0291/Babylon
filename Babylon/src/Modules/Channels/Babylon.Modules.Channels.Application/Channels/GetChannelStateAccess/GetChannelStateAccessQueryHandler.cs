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
            $"""
            SELECT
              cm.is_mute AS {nameof(ChannelAccessStateDto.IsMute)}
              c.type AS {nameof(ChannelAccessStateDto.Type)}
            FROM channels.channel_members cm
            JOIN channels.channels c ON c.channel_id = cm.channel_id
            WHERE cm.id = @Id AND cm.channel_id = @ChannelId
            """;

        ChannelAccessStateDto channelState = await connection.QuerySingleAsync<ChannelAccessStateDto>(sql, request);

        return Result.Success(channelState);
    }
}
