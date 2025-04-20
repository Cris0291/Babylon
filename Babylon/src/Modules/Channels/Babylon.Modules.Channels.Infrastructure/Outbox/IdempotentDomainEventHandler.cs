using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Common.Infrastructure.Outbox;
using Dapper;

namespace Babylon.Modules.Channels.Infrastructure.Outbox;
internal sealed class IdempotentDomainEventHandler<TDomainEvent>(IDomainEventHandler<TDomainEvent> decorated, IDbConnectionFactory dbConnectionFactory) : DomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    public override async Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        var outboxMessageConsumer = new OutboxMessageConsumer(domainEvent.Id, decorated.GetType().Name);

        if(await OutboxConsumerExistAsync(connection, outboxMessageConsumer))
        {
            return;
        }

        await decorated.Handle(domainEvent, cancellationToken);

        await InsertOutboxCoonsumerAsync(connection, outboxMessageConsumer);
    }
    private static async Task<bool> OutboxConsumerExistAsync(DbConnection connection, OutboxMessageConsumer outboxMessageConsumer)
    {
        const string sql =
            """
            SELECT EXISTS(
                SELECT 1
                FROM channels.outbox_message_consumers
                WHERE outbox_message_id = @OutboxMessageId AND
                      name = @NAME)
            """;

        return await connection.ExecuteScalarAsync<bool>(sql, outboxMessageConsumer);
    }
    private static async Task InsertOutboxCoonsumerAsync(DbConnection connection, OutboxMessageConsumer outboxMessageConsumer)
    {
        const string sql =
            """
            INSERT INTO channels.outbox_message_consumers(outbox_message_id, name)
            VALUE (@OutboxMessageId, @Name)
            """;

       await connection.ExecuteAsync(sql, outboxMessageConsumer);
    }
}
