using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Dapper;

namespace Babylon.Modules.Channels.Application.Threads.GetThreadsList;

internal sealed class GetThreadsListQueryHandler(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<GetThreadsListQuery, List<Guid>>
{
    public async Task<Result<List<Guid>>> Handle(GetThreadsListQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();
        
        const string sql = 
            $"""
             SELECT tc.thread_channel.id
             FROM channels.thread_channels tc 
             WHERE tc.channel_id = @ChannelId
             """;
        
        IEnumerable<Guid> threadIds = await connection.QueryAsync<Guid>(sql, new {request.ChannelId});

        return Result.Success(threadIds.ToList());
    }
}
