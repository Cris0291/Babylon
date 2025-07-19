using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.Channels;
using Dapper;

namespace Babylon.Modules.Channels.Application.Channels.RenameChannel;
internal sealed class RenameChannelCommandHandler(IChannelRepository channelRepository, IUnitOfWork unitOfWork) : ICommandHandler<RenameChannelCommand>
{
    public async Task<Result> Handle(RenameChannelCommand request, CancellationToken cancellationToken)
    {
        Channel? channel = await channelRepository.GetChannel(request.ChannelId);
        
        if (channel is null)
        {
            return Result.Failure(Error.Failure(description: "Channel was not found"));
        }

        bool isAdmin = channel.IsAdmin(request.Id);

        if (!isAdmin)
        {
            return Result.Failure(Error.Failure(description: "Only the channel creator can perform this action"));
        }

        channel!.Rename(request.Name);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
