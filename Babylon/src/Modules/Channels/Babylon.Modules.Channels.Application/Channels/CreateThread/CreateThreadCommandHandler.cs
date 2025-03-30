using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.Channels;
using Babylon.Modules.Channels.Domain.MessageThreadChannels;
using Babylon.Modules.Channels.Domain.ThreadChannels;

namespace Babylon.Modules.Channels.Application.Channels.CreateThread;
internal sealed class CreateThreadCommandHandler(IUnitOfWork unitOfWork, IChannelRepository channelRepository, IThreadChannelRepository threadChannelRepository, IMessageThreadChannelRepository messageRepository) : ICommandHandler<CreateThreadCommand>
{
    public async Task<Result> Handle(CreateThreadCommand request, CancellationToken cancellationToken)
    {
        Channel? channel = await channelRepository.GetChannel(request.ChannelId);
        if (channel is null)
        {
            throw new InvalidOperationException("Channel was not found");
        }

        var threadChannel = ThreadChannel.Create(request.ChannelName, request.ChannelId);
        await threadChannelRepository.Insert(threadChannel);

        var message = MessageThreadChannel.Create(request.UserName, request.MessageText, threadChannel.ThreadChannelId, request.MemberId, request.CreationDate);
        await messageRepository.Insert(message);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
