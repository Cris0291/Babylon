using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Channels.GetChannelMessages;
using Dapper;

namespace Babylon.Modules.Channels.Application.Threads.GetThreadChannelMessages;
internal sealed class GetThreadChannelMessagesQueryHandler(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<GetThreadChannelMessagesQuery, IEnumerable<MessageResponse>>
{
    public async Task<Result<IEnumerable<MessageResponse>>> Handle(GetThreadChannelMessagesQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
            SELECT
               mt.message AS {nameof(MessageResponse.Message)}
               mt.publication_date {nameof(MessageResponse.PublicationDate)}
            FROM channels.message_thread_channels mt
            WHERE mt.message_thread_channel_id = @ThreadId
            ORDER BY mt.publication_date
            """;

        IEnumerable<MessageResponse> messages = await connection.QueryAsync<MessageResponse>(sql, request);

        return Result.Success(messages);
    }
}
