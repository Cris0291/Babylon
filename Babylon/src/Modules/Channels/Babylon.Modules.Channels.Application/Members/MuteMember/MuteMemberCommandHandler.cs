using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.Channels;
using Dapper;

namespace Babylon.Modules.Channels.Application.Members.MuteMember;
internal sealed class MuteMemberCommandHandler(IChannelMemberRepository channelMemberRepository, IUnitOfWork unitOfWork, IDbConnectionFactory dbConnectionFactory) : ICommandHandler<MuteMemberCommand>
{
    public async Task<Result> Handle(MuteMemberCommand request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            """
            SELECT EXIST(
               SELECT 1
               FROM channels.channels c
               WHERE c.creator = @Id AND c.channel_id = @ChannelId
            )
            """;

        bool isAdmin = await connection.ExecuteScalarAsync<bool>(sql, new { request.ChannelId, Id = request.AdminId });

        if (!isAdmin)
        {
            throw new InvalidOperationException("Requested action (mute a member) can only be performed by an admin");
        }

        ChannelMember? member = await channelMemberRepository.GetChannelMember(request.ChannelId, request.Id);

        if(member == null)
        {
            throw new InvalidOperationException("The target member was not found");
        }

        member.MuteMember();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
