using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Dapper;

namespace Babylon.Modules.Channels.Application.Channels.GetChannelMessages;
internal sealed partial class GetChannelMessagesQueryHandler(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<GetChannelMessagesQuery, IEnumerable<MessageResponse>>
{
    public async Task<Result<IEnumerable<MessageResponse>>> Handle(GetChannelMessagesQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
           $"""
            SELECT
               m.message AS {nameof(MessageResponse.Message)}
               m.publication_date {nameof(MessageResponse.PublicationDate)}
            FROM channels.message_channels m
            WHERE m.message_channel_id = @Id
            ORDER BY m.publication_date
            """;

        IEnumerable<MessageResponse> messages = await connection.QueryAsync<MessageResponse>(sql, request);

        return Result.Success(messages);
    }
}
