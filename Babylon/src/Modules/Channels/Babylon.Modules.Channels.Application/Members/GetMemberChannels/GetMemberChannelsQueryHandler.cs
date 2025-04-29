using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Dapper;

namespace Babylon.Modules.Channels.Application.Members.GetMemberChannels;
internal sealed partial class GetMemberChannelsQueryHandler(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<GetMemberChannelsQuery, IEnumerable<MemberChannelsDto>>
{
    public async Task<Result<IEnumerable<MemberChannelsDto>>> Handle(GetMemberChannelsQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
            SELECT  
               c.channel_id AS {nameof(MemberChannelsDto.ChannelId)}
               c.name AS {nameof(MemberChannelsDto.Name)}
            FROM channels.channel_members cm
            JOIN channels.members m ON cm.id = m.id
            JOIN channels.channels c ON c.channel_id = cm.channel_id 
            WHERE m.id = @MemberId
            """;

        IEnumerable<MemberChannelsDto> channels = await connection.QueryAsync<MemberChannelsDto>(sql, request);

        return channels.AsList();
    }
}
