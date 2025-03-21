using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Domain.Channels;

namespace Babylon.Modules.Channels.Application.Channels.GetChannels;
internal sealed class GetChannelsQueryHandler(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<GetChannelsQuery, IEnumerable<Channel>>
{
    public async Task<Result<IEnumerable<Channel>>> Handle(GetChannelsQuery request, CancellationToken cancellationToken)
    {
        DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql = $""" """;
    }
}
