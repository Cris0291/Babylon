using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Dapper;

namespace Babylon.Modules.Channels.Application.Members.GetMemberMute;
internal sealed class GetMemberMuteQueryHandler(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<GetMemberMuteQuery, bool>
{
    public async Task<Result<bool>> Handle(GetMemberMuteQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
            SELECT
               cm.is_mute AS {nameof(ChannelMemberDto.IsMute)}
            FROM channels.channel_members cm
            WHERE cm.id = @Id AND m.channel_id
            """;

        connection.QueryAsync
    }
    internal record ChannelMemberDto(bool IsMute);
}
