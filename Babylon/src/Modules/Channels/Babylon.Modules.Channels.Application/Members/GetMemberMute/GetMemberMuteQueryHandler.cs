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
            WHERE cm.id = @Id AND cm.channel_id = @ChannelId
            """;

        ChannelMemberDto? member = await connection.QuerySingleOrDefaultAsync<ChannelMemberDto>(sql, new {request.ChannelId, request.Id});

        if (member == null)
        {
            return Result.Failure<bool>(Error.Failure(description: "Target user was not found for given channel"));
        }

        return Result.Success<bool>(member.IsMute);
    }
    internal record ChannelMemberDto(bool IsMute);
}
