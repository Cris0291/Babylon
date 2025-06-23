using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.Channels;

namespace Babylon.Modules.Channels.Application.Channels.ArchiveChannel;
internal sealed class ArchiveChannelCommandHandler(IUnitOfWork unitOfWork, IChannelRepository channelRepository) : ICommandHandler<ArchiveChannelCommand>
{
    public async Task<Result> Handle(ArchiveChannelCommand request, CancellationToken cancellationToken)
    {
        Channel? channel = await channelRepository.GetChannel(request.ChannelId);

        if (channel is null)
        {
            return Result.Failure(Error.Failure(description: "Channel was not found"));
        }

        bool isAdmin = channel.IsAdmin(request.AdminId);

        if (!isAdmin)
        {
            return Result.Failure(Error.Failure(description: "Only the channel creator can perform this action"));
        }
        
        Result result = channel.ArchiveChannel();
        
        if (!result.IsSuccess)
        {
            return result;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
