using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.MessageThreadChannels;

namespace Babylon.Modules.Channels.Application.ThreadMessages.CreateThreadMessage;
internal sealed class CreateThreadMessageCommandHandler(IMessageThreadChannelRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<CreateThreadMessageCommand>
{
    public async Task<Result> Handle(CreateThreadMessageCommand request, CancellationToken cancellationToken)
    {
        var message = MessageThreadChannel.Create(
            request.ThreadId, 
            request.MemberId,
            request.Message,
            request.UserName,
            request.Avatar,
            request.PublicationDate
            );

        await repository.Insert(message);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
