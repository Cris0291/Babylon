using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Domain.MessageChannels;

namespace Babylon.Modules.Channels.Application.Messages.CreateMessage;
internal sealed class CreateMessageCommandHandler(IMessageChannelRepository messageChannelRepository) : ICommandHandler<CreateMessageCommand>
{
    public async Task<Result> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        var message = MessageChannel.Create(
            request.ChannelId, 
            request.MemberId, 
            request.Message, 
            request.PublicationDate,
            request.UserName, 
            request.Avatar);

        await messageChannelRepository.Insert(message);

        return Result.Success();
    }
}
