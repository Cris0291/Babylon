using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Domain.Channels;
using Dapper;

namespace Babylon.Modules.Channels.Application.Channels.GetChannels;
internal sealed class GetChannelsQueryHandler(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<GetChannelsQuery, IEnumerable<ChannelsResponse>>
{
    public async Task<Result<IEnumerable<ChannelsResponse>>> Handle(GetChannelsQuery request, CancellationToken cancellationToken)
    {
        DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        var parameters = new GetChannelsParameters(request.Name, request.Type);

        const string sql = $"""
                            SELECT 
                            channel_id AS {nameof(ChannelsResponse.Id)}
                            name As {nameof(ChannelsResponse.Name)}
                            type As {nameof(ChannelsResponse.Type)}
                            FROM channels.channels
                            WHERE name LIKE %@NAME% AND
                            (type == @TYPE)
                            ORDER BY channel_id
                            """;
        IEnumerable<ChannelsResponse> result = await connection.QueryAsync<ChannelsResponse>(sql, parameters);
        return Result.Success(result);
    }
}
