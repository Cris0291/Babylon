using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Domain.ThreadChannels;

namespace Babylon.Modules.Channels.Application.Threads.RenameThread;

internal sealed class RenameThreadChannelCommandHandler(IThreadChannelRepository threadChannelRepository) : ICommandHandler<RenameThreadChannelCommand>
{
    public async Task<Result> Handle(RenameThreadChannelCommand request, CancellationToken cancellationToken)
    {
        ThreadChannel threadChannel = await threadChannelRepository.Get(request.ThreadChannelId);
        if (threadChannel == null)
        {
            throw new InvalidOperationException("Requested thread was not found");
        }
        threadChannel.Rename(request.ThreadChannelName);
        
        return Result.Success();
    }
}
