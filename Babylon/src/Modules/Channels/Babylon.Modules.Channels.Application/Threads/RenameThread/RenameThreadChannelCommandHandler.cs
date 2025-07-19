using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.ThreadChannels;
using Dapper;

namespace Babylon.Modules.Channels.Application.Threads.RenameThread;

internal sealed class RenameThreadChannelCommandHandler(IThreadChannelRepository threadChannelRepository, IUnitOfWork unitOfWork) : ICommandHandler<RenameThreadChannelCommand>
{
    public async Task<Result> Handle(RenameThreadChannelCommand request, CancellationToken cancellationToken)
    {
        ThreadChannel threadChannel = await threadChannelRepository.Get(request.ThreadChannelId);
        
        threadChannel!.Rename(request.ThreadChannelName);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
