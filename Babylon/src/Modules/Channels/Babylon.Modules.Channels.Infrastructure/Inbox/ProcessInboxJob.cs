using System.Data;
using System.Data.Common;
using Babylon.Common.Application.Data;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;

namespace Babylon.Modules.Channels.Infrastructure.Inbox;

[DisallowConcurrentExecution]
internal sealed class ProcessInboxJob(
    IDbConnectionFactory dbConnection, 
    IServiceScopeFactory serviceScopeFactory, 
    IOptions<InboxOptions> options) 
    : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await using DbConnection connection = await dbConnection.OpenConnectionAsync();
        await using DbTransaction transaction = await connection.BeginTransactionAsync();

        IReadOnlyList<InboxMessageResponse> inboxMessages = await GetInboxMessagesAsync(connection, transaction);
    }
    private async Task<IReadOnlyList<InboxMessageResponse>> GetInboxMessagesAsync(IDbConnection connection, IDbTransaction transaction)
    {
        string sql =
            $"""
            SELECT 
               id AS {nameof(InboxMessageResponse.Id)}
               content AS {nameof(InboxMessageResponse.Content)}
            FROM channels.inbox_messages
            WHERE processed_on_utc IS NULL
            ORDER BY occurred_on_utc
            LIMIT {options.Value.BatchSize}
            WITH (UPDLOCK)
            """;

        IEnumerable<InboxMessageResponse> inboxMessageResponses = await connection.QueryAsync<InboxMessageResponse>(sql, transaction: transaction);

        return inboxMessageResponses.AsList();
    }
    internal sealed record InboxMessageResponse(Guid Id, string Content);
}
