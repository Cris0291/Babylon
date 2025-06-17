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
               mt.message AS {nameof(MessageThreadChannelResponse.Message)}
               mt.publication_date AS {nameof(MessageThreadChannelResponse.PublicationDate)}
               mt.user_name AS {nameof(MessageThreadChannelResponse.UserName)}
               mt.avatar_url AS {nameof(MessageThreadChannelResponse.Avatar)}
               mt.number_of_likes AS {nameof(MessageThreadChannelResponse.NumberOfLikes)}
               mt.number_of_dislikes AS {nameof(MessageThreadChannelResponse.NumberOfDislikes)}
               mu.like AS {nameof(MessageThreadChannelResponse.UserLike)}
               mu.dislike AS {nameof(MessageThreadChannelResponse.UserDislike)}
               mu.pin AS {nameof(MessageThreadChannelResponse.UserPin)}
               STRING_AGG(mtr.emoji) AS {nameof(MessageThreadChannelResponse.Emojis)}
            FROM channels.message_thread_channels mt
            JOIN channels.message_thread_channel_reactions mtr ON mtr.message_thread_channel_id = mt.message_thread_channel_id
            JOIN channels.message_thread_channel_reactions mu ON mu.message_thread_channel_id = mt.message_thread_channel_id AND mu.id = mt.id 
            WHERE mt.message_thread_channel_id = @ThreadId
            ORDER BY mt.publication_date
            """;

        IEnumerable<MessageResponse> messages = await connection.QueryAsync<MessageResponse>(sql, request);

        return Result.Success(messages);
    }
}
