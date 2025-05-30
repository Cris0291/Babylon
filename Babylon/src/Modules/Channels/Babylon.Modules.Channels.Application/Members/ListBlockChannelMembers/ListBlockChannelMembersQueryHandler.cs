using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Domain.Channels;
using Dapper;

namespace Babylon.Modules.Channels.Application.Members.ListBlockChannelMembers;
internal sealed class ListBlockChannelMembersQueryHandler(IChannelRepository channelRepository, IDbConnectionFactory dbConnectionFactory) : IQueryHandler<ListBlockChannelMembersQuery, IEnumerable<BlockMemberDto>>
{
    public async Task<Result<IEnumerable<BlockMemberDto>>> Handle(ListBlockChannelMembersQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        Channel? channel = await channelRepository.GetChannel(request.ChannelId);

        if (channel == null)
        {
            return Result.Failure<IEnumerable<BlockMemberDto>>(Error.Failure(description: "Requested channel was not found"));
        }

        bool isAdmin = channel.IsAdmin(request.AdminId);

        if (!isAdmin)
        {
            return Result.Failure<IEnumerable<BlockMemberDto>>(Error.Failure(description: "Administrator level permission is required to perform given action"));
        }

        const string sql =
            $"""
            SELECT
               id AS {nameof(BlockMemberDto.Id)}
               first_name AS {nameof(BlockMemberDto.FirstName)}
               last_name AS {nameof(BlockMemberDto.LastName)}
               email AS {nameof(BlockMemberDto.Email)}
            FROM channels.members
            WHERE id IN @Ids
            """;

        IEnumerable<BlockMemberDto> blockedMembers = await connection.QueryAsync<BlockMemberDto>(sql, new {Ids = channel.BlockedMembers});
        return Result.Success(blockedMembers);
    }
}
