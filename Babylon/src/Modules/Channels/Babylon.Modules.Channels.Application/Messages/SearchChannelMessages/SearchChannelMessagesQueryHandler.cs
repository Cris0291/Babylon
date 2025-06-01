using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;

namespace Babylon.Modules.Channels.Application.Messages.SearchChannelMessages;
internal sealed class SearchChannelMessagesQueryHandler(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<SearchChannelMessagesQuery, IEnumerable<SearchedChannelMessageDto>>
{
    public async Task<Result<IEnumerable<SearchedChannelMessageDto>>> Handle(SearchChannelMessagesQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql = 
            """
            SELECT
            FROM
            """;
    }
}
