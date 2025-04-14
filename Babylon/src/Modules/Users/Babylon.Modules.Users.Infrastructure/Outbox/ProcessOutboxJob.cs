using System.Data;
using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Common.Infrastructure.Outbox;
using Babylon.Common.Infrastructure.Serialization;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;

namespace Babylon.Modules.Users.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxJob
    (
    IDbConnectionFactory dbConnectionFactory,
    IServiceScopeFactory serviceScopeFactory,
    IOptions<OutboxOptions> outboxOptions
    ) : IJob
{
    //private const string ModuleName = "Users";
    public async Task Execute(IJobExecutionContext context)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();
        await using DbTransaction transaction = await connection.BeginTransactionAsync();

        IReadOnlyList<OutboxMessageResponse> outboxMessages = await GetOutboxMessagesAsync(connection, transaction);

        foreach (OutboxMessageResponse message in outboxMessages)
        {
            Exception? exception = null;
            try
            {
                IDomainEvent domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(message.Content, SerializationSettings.Instance)!;

                 using IServiceScope scope = serviceScopeFactory.CreateScope();

                IEnumerable<IDomainEventHandler> handlers = DomainEventHandlersFactory.GetHandlers(
                    domainEvent.GetType(), 
                    scope.ServiceProvider, 
                    Application.AssemblyReference.Assembly);

                foreach (IDomainEventHandler domainEventHandler in handlers)
                {
                    await domainEventHandler.Handle(domainEvent);
                }
            }
            catch(Exception caughtException)
            {
                exception = caughtException;
            }

            await UpdateOutboxMessageAsync(connection, transaction, message, exception);
        }

        await transaction.CommitAsync();
    }
    private async Task<IReadOnlyList<OutboxMessageResponse>> GetOutboxMessagesAsync(IDbConnection connection, IDbTransaction transaction)
    {
        string sql =
            $"""
            SELECT
               id AS {nameof(OutboxMessageResponse.Id)}
               content AS {nameof(OutboxMessageResponse.Content)}
            FROM users.outbox_messages
            WHERE processed_on_utc IS NULL
            ORDER BY occurred_on_utc
            LIMIT {outboxOptions.Value.BatchSize}
            WITH (UPDLOCK)
            """;

        IEnumerable<OutboxMessageResponse> messages = await connection.QueryAsync<OutboxMessageResponse>(sql, transaction: transaction);

        return messages.ToList();
    }
    private async Task UpdateOutboxMessageAsync(IDbConnection connection, IDbTransaction transaction, OutboxMessageResponse outboxMessage, Exception? exception)
    {
        const string sql =
            """
            UPDATE users.outbox_messages
            SET processed_on_utc = @ProcessedOnUtc,
                error = @Error
            Where id = @Id
            """;

        await connection.ExecuteAsync(
            sql,
            new
            {
                outboxMessage.Id,
                ProcessedOnUtc = DateTime.Now,
                Error = exception?.ToString()
            },
            transaction);
        ;
    }
    internal sealed record OutboxMessageResponse(Guid Id, string Content);
}
