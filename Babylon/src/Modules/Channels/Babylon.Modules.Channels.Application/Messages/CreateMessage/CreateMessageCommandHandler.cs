using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.MessageChannels;

namespace Babylon.Modules.Channels.Application.Messages.CreateMessage;
internal sealed class CreateMessageCommandHandler(IMessageChannelRepository messageChannelRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateMessageCommand>
{
    public async Task<Result> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        var message = MessageChannel.Create(
            request.ChannelId, 
            request.Id, 
            request.Message, 
            request.PublicationDate,
            request.UserName, 
            request.Avatar);

        await messageChannelRepository.Insert(message);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
