using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.EventBus;
using Babylon.Common.Infrastructure.Inbox;
using Babylon.Common.Infrastructure.Serialization;
using Dapper;
using MassTransit;
using Newtonsoft.Json;

namespace Babylon.Modules.Users.Infrastructure.Inbox;
internal sealed class IntegrationEventConsumer<TIntegrationEvent>(IDbConnectionFactory dbConnectionFactory) : IConsumer<TIntegrationEvent>
    where TIntegrationEvent : IntegrationEvent
{
    public async Task Consume(ConsumeContext<TIntegrationEvent> context)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        TIntegrationEvent integrationEvent = context.Message;

        var inboxMessage = new InboxMessage
        {
            Id = integrationEvent.Id,
            Type = integrationEvent.GetType().Name,
            Content = JsonConvert.SerializeObject(integrationEvent, SerializationSettings.Instance),
            OccurredOnUtc = integrationEvent.OccurredOnUtc
        };

        const string sql =
            """
            INSERT INTO users.inbox_messages(id, type, content, occurred_on_utc)
            VALUES (@Id, @Type, @Content, @OccurredOnUtc)
            """;

        await connection.ExecuteAsync(sql, inboxMessage);
    }
}
