using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.ThreadChannels;
using Dapper;

namespace Babylon.Modules.Channels.Application.Threads.ArchiveThread;

internal sealed class ArchiveThreadChannelCommandHandler(IDbConnectionFactory dbConnectionFactory, IThreadChannelRepository threadChannelRepository, IUnitOfWork unitOfWork) : ICommandHandler<ArchiveThreadChannelCommand>
{
    public async Task<Result> Handle(ArchiveThreadChannelCommand request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();
        
        const string sql =
            """
            WITH SelectedChannel AS (
                SELECT 
                  channel_id, 
                  type, 
                  creator
                FROM channels.channels
                WHERE channel_id = @ChannelId
            ),
            AdminChannel AS (
                SELECT
                  sc.channel_id,
                  sc.type,
                  sc.creator,
                  CASE 
                    WHEN EXISTS (SELECT 1 FROM SelectedChannel) THEN 1 
                    ELSE 0 
                  END AS ExistChannel,
                  CASE 
                    WHEN sc.creator = @AdminId THEN 1 
                    ELSE 0 
                  END AS IsAdmin
                FROM SelectedChannel sc
            ),
            SelectedThread AS (
                SELECT
                  ad.channel_id,
                  ad.type,
                  ad.creator,
                  ad.ExistChannel,
                  ad.IsAdmin,
                  CASE
                    WHEN EXISTS (
                      SELECT 1
                      FROM channels.thread_channels tc
                      WHERE tc.channel_id        = ad.channel_id
                        AND tc.thread_channel_id = @ThreadChannelId
                    ) THEN 1
                    ELSE 0
                  END AS ExistThread
                FROM AdminChannel ad
            )
            SELECT 
              ExistChannel, 
              IsAdmin, 
              ExistThread
            FROM SelectedThread;
            """;

        (int existChannel, int isAdmin, int existThread) = await connection.QuerySingleAsync<(int existChannel, int isAdmin, int existThread)>(sql,
            new { request.ThreadChannelId, request.AdminId, request.ChannelId });

        if (existChannel == 0)
        {
            return Result.Failure(Error.Failure(description: "Channel not found"));
        }

        if (isAdmin == 0)
        {
            return Result.Failure(Error.Failure(description: "Only the channel creator can perform this action"));
        }

        if (existThread == 0)
        {
            return Result.Failure(Error.Failure(description: "Thread not found in this channel"));
        }

        ThreadChannel? threadChannel = await threadChannelRepository.Get(request.ThreadChannelId);
    }
}
