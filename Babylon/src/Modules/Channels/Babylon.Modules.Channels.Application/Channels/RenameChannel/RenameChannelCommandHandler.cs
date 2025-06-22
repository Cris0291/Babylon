using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.Channels;
using Dapper;

namespace Babylon.Modules.Channels.Application.Channels.RenameChannel;
internal sealed class RenameChannelCommandHandler(IChannelRepository channelRepository, IDbConnectionFactory dbConnectionFactory , IUnitOfWork unitOfWork) : ICommandHandler<RenameChannelCommand>
{
    public async Task<Result> Handle(RenameChannelCommand request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();
        
        const string sql =
            """
            WITH SelectedChannel AS (
                SELECT channel_id, type
                FROM channels.channels
                WHERE channel_id = @ChannelId
            ),
            Authorized AS (
                SELECT
                   sc.channel_id,
                   sc.type,
                   CASE WHEN EXISTS (SELECT 1 FROM SelectedChannel) THEN 1 ELSE 0 END AS ExistFLag,
                   CASE
                       WHEN EXISTS (
                           SELECT 1
                           FROM channels.channel_members cm
                           WHERE cm.channel_id = @ChannelId AND cm.id = @Id
                       ) THEN 1
                       ELSE 0
                   END AS IsAuthorized
                FROM SelectedChannel sc
            ),
            SELECT IsAuthorized, ExistFLag
            FROM Authorized
            """;
        
        (int existsFlag, int isAuthorized) = await connection.QuerySingleAsync<(int ExistFlag, int IsAuthorized)>(sql, new { request.Id, request.ChannelId});
        
        if (existsFlag == 0)
        {
            throw new InvalidOperationException("Requested channel was not found");
        }

        if (isAuthorized == 0)
        {
            throw new InvalidOperationException("Not authorized");
        }
        
        Channel? channel = await channelRepository.GetChannel(request.ChannelId);

        channel!.Rename(request.Name);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
