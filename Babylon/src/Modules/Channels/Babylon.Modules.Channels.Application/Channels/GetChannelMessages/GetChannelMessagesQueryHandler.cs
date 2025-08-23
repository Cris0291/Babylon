using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Dapper;

namespace Babylon.Modules.Channels.Application.Channels.GetChannelMessages;
internal sealed class GetChannelMessagesQueryHandler(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<GetChannelMessagesQuery, IEnumerable<MessageResponse>>
{
    public async Task<Result<IEnumerable<MessageResponse>>> Handle(GetChannelMessagesQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
           $"""
            SELECT
              m.message AS {nameof(MessageResponse.Message)},
              m.publication_date AS {nameof(MessageResponse.PublicationDate)},
              m.user_name AS {nameof(MessageResponse.UserName)},
              m.avatar_url AS {nameof(MessageResponse.Avatar)},
              m.number_of_likes AS {nameof(MessageResponse.NumberOfLikes)},
              m.number_of_dislikes AS {nameof(MessageResponse.NumberOfDislikes)},
              ISNULL(mu.like, 0) AS {nameof(MessageResponse.UserLike)},
              ISNULL(mu.dislike, 0) AS {nameof(MessageResponse.UserDislike)},
              mu.pin AS {nameof(MessageResponse.UserPin)},
              ISNULL(STRING_AGG(mtr.emoji, ','), '') AS {nameof(MessageResponse.Emojis)}
            FROM channels.message_channels m
            LEFT JOIN channels.message_channel_reactions mtr ON mtr.message_channel_id = m.message_channel_id
            LEFT JOIN channels.message_channel_reactions mu ON mu.message_channel_id = m.message_channel_id AND mu.id = @Id 
            WHERE m.channel_id = @ChannelId
            GROUP BY
               m.message_channel_id,
               m.message,
               m.publication_date,
               m.user_name,
               m.avatar_url,
               m.number_of_likes,
               m.number_of_dislikes,
               ISNULL(mu.[like], 0),
               ISNULL(mu.[dislike], 0),
               mu.pin, 0
            ORDER BY m.publication_date
            """;

        IEnumerable<MessageResponse> messages = await connection.QueryAsync<MessageResponse>(sql, request);

        return Result.Success(messages);
    }
}
