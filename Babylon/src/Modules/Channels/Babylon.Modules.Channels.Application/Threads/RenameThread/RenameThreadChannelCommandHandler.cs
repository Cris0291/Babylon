using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.ThreadChannels;
using Dapper;

namespace Babylon.Modules.Channels.Application.Threads.RenameThread;

internal sealed class RenameThreadChannelCommandHandler(IThreadChannelRepository threadChannelRepository, IDbConnectionFactory dbConnectionFactory , IUnitOfWork unitOfWork) : ICommandHandler<RenameThreadChannelCommand>
{
    public async Task<Result> Handle(RenameThreadChannelCommand request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();
        
        const string sql = 
            """
            WITH SelectedThreadChannel AS (
                SELECT tc.thread_channel_id AS Tid 
                FROM channels.thread_channels tc
                WHERE tc.thread_channel_id = @ThreadChannelId
            )
            AuthorizedThread AS (
                 SELECT
                     st.Tid,
                     CASE
                       WHEN EXISTS (SELECT 1 FROM SelectedThreadChannel) THEN 1 ELSE 0 END AS ExistFlag,
                     CASE
                       WHEN EXISTS (
                          SELECT 1
                          FROM channels.thread_channel_members tcm
                          WHERE tcm.thread_channel_id = @ThreadChannelId AND tcm.id = @Id  
                       ) 
                       THEN 1
                       ELSE 0
                     END AS IsAuthorized
                 FROM  SelectedThreadChannel st
            )
            SELECT IsAuthorized, ExistFlag
            FROM AuthorizedThread
            """;

        (int existsFlag, int isAuthorized) = await connection.QuerySingleAsync<(int ExistFlag, int IsAuthorized)>(sql, new { request.Id, request.ThreadChannelId});

        if (existsFlag == 0)
        {
            throw new InvalidOperationException("Requested thread was not found");
        }

        if (isAuthorized == 0)
        {
            throw new InvalidOperationException("Not authorized");
        }
        
        ThreadChannel threadChannel = await threadChannelRepository.Get(request.ThreadChannelId);
        
        threadChannel!.Rename(request.ThreadChannelName);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
