using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Channels.GetChannelMessages;

namespace Babylon.Modules.Channels.Application.DirectedChannels.GetDirectedChannelMessages;

public class GetDirectedChannelMessagesQueryHandler(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<GetDirectedChannelMessagesQuery,IEnumerable<MessageResponse>>
{
    public async Task<Result<IEnumerable<MessageResponse>>> Handle(GetDirectedChannelMessagesQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection dbConnection = await dbConnectionFactory.OpenConnectionAsync();
        
        const string sql =
                   $"""
                    SELECT
                      m.message AS {nameof(MessageResponse.Message)}
                      m.publication_date AS {nameof(MessageResponse.PublicationDate)}
                      m.user_name AS {nameof(MessageResponse.UserName)}
                      m.avatar_url AS {nameof(MessageResponse.Avatar)}
                      m.number_of_likes AS {nameof(MessageResponse.NumberOfLikes)}
                      m.number_of_dislikes AS {nameof(MessageResponse.NumberOfDislikes)}
                      mu.like AS {nameof(MessageResponse.UserLike)}
                      mu.dislike AS {nameof(MessageResponse.UserDislike)}
                      mu.pin AS {nameof(MessageResponse.UserPin)}
                      STRING_AGG(mtr.emoji) AS {nameof(MessageResponse.Emojis)}
                    FROM channels.message_directed_channels m
                    LEFT JOIN channels.message_directed_channel_reactions mtr ON mtr.message_channel_id = m.message_channel_id
                    LEFT JOIN channels.message_directed_channel_reactions mu ON mu.message_channel_id = m.message_channel_id AND mu.id = @Id 
                    WHERE m.message_channel_id = @ChannelId
                    GROUP BY
                        m.message,
                        m.publication_date,
                        m.user_name,
                        m.avatar_url,
                        m.number_of_likes,
                        m.number_of_dislikes,
                        mu.like,
                        mu.dislike,
                        mu.pin
                    ORDER BY m.publication_date
                    """;
    }
}
