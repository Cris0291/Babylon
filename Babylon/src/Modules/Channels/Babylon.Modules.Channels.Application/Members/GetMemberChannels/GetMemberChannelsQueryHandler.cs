using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;

namespace Babylon.Modules.Channels.Application.Members.GetMemberChannels;
internal sealed class GetMemberChannelsQueryHandler(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<GetMemberChannelsQuery, IEnumerable<int>>
{
    public async Task<Result<IEnumerable<int>>> Handle(GetMemberChannelsQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();
        const string sql =
            """
            SELECT  
            """;
    }
}
