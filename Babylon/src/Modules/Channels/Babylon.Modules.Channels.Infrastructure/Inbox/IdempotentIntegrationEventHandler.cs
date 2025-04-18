using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.EventBus;
using Babylon.Common.Infrastructure.Inbox;
using Dapper;

namespace Babylon.Modules.Channels.Infrastructure.Inbox;
internal sealed class IdempotentIntegrationEventHandler<TIntegrationEvent>(
    IIntegrationEventHandler<TIntegrationEvent> decorated, 
    IDbConnectionFactory dbConnectionFactory) 
    : IntegrationEventHandler<TIntegrationEvent>
    where TIntegrationEvent : IntegrationEvent
{
    public override async Task Handle(TIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        var inboxMessageConsumer = new InboxMessageConsumer(integrationEvent.Id, decorated.GetType().Name);

        if(await InboxConsumerExistAsync(connection, inboxMessageConsumer))
        {
            return;
        }

        await decorated.Handle(integrationEvent, cancellationToken);

        await InsertInboxConsumerAsync(connection, inboxMessageConsumer);
    }
    private static async Task<bool> InboxConsumerExistAsync(DbConnection connection, InboxMessageConsumer inboxMessageConsumer)
    {
        const string sql =
            """
            SELECT EXIST(
               SELECT 1
               FROM channels.inbox_message_consumers
               WHERE inbox_message_id = @ID AND
                     name = @NAME
            )
            """;

        return await connection.ExecuteScalarAsync<bool>(sql, inboxMessageConsumer);
    }
    private static async Task InsertInboxConsumerAsync(DbConnection connection, InboxMessageConsumer inboxMessageConsumer)
    {
        const string sql =
            """
            INSERT INTO channels.inbox_message_consumers (inbox_message_id, name)
            VALUES (@InboxMessageId, @Name)
            """;

        await connection.ExecuteAsync(sql,inboxMessageConsumer);
    }
}
