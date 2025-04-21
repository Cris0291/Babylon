using System.Data;
using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.EventBus;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;
using Babylon.Common.Infrastructure.Serialization;
using Babylon.Common.Infrastructure.Inbox;

namespace Babylon.Modules.Users.Infrastructure.Inbox;

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

        foreach (InboxMessageResponse inboxMessage in inboxMessages)
        {
            Exception? exception = null;
            try
            {
                IIntegrationEvent integrationEvent = JsonConvert.DeserializeObject<IIntegrationEvent>(
                    inboxMessage.Content,
                    SerializationSettings.Instance)!;

                using IServiceScope scope = serviceScopeFactory.CreateScope();

                IEnumerable<IIntegrationEventHandler> integrationEventHandlers = IntegrationEventHandlersFactory.GetHandlers(
                    integrationEvent.GetType(),
                    scope.ServiceProvider,
                    Presentation.AssemblyReference.Assembly);

                foreach (IIntegrationEventHandler integrationEventHandler in integrationEventHandlers)
                {
                    await integrationEventHandler.Handle(integrationEvent, context.CancellationToken);
                }
            }
            catch (Exception caughtException)
            {
                exception = caughtException;
            }

            await UpdateInboxMessageAsync(connection, transaction, inboxMessage, exception);
        }

        await transaction.CommitAsync();
    }
    private async Task<IReadOnlyList<InboxMessageResponse>> GetInboxMessagesAsync(IDbConnection connection, IDbTransaction transaction)
    {
        string sql =
            $"""
            SELECT 
               id AS {nameof(InboxMessageResponse.Id)}
               content AS {nameof(InboxMessageResponse.Content)}
            FROM users.inbox_messages
            WHERE processed_on_utc IS NULL
            ORDER BY occurred_on_utc
            LIMIT {options.Value.BatchSize}
            WITH (UPDLOCK)
            """;

        IEnumerable<InboxMessageResponse> inboxMessageResponses = await connection.QueryAsync<InboxMessageResponse>(sql, transaction: transaction);

        return inboxMessageResponses.AsList();
    }
    private async Task UpdateInboxMessageAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        InboxMessageResponse inboxMessage,
        Exception? exception
        )
    {
        const string sql =
            """
            UPDATE users.inbox_messages
            SET processed_on_utc = @ProcessedOnUtc
                error = @Error
            WHERE id = @Id
            """;

        await connection.ExecuteAsync(
            sql,
            new
            {
                id = inboxMessage.Id,
                ProcessedOnUtc = DateTime.Now,
                Error = exception?.ToString()
            },
            transaction: transaction
            );
    }
    internal sealed record InboxMessageResponse(Guid Id, string Content);
}
