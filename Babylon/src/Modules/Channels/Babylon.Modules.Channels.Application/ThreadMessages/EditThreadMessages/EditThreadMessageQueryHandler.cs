using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.MessageThreadChannels;

namespace Babylon.Modules.Channels.Application.ThreadMessages.EditThreadMessages;

internal sealed class EditThreadMessageQueryHandler(IMessageThreadChannelRepository messageThreadChannelRepository, IUnitOfWork unitOfWork) : ICommandHandler<EditThreadMessageQuery>
{
    public async Task<Result> Handle(EditThreadMessageQuery request, CancellationToken cancellationToken)
    {
        MessageThreadChannel? threadMessage = await messageThreadChannelRepository.Get(request.MessageThreadChannelId);
        
        if(threadMessage == null)
        {
            return Result.Failure(Error.Failure(description: "Requested message was not found"));
        }
        
        threadMessage.Edit(request.Message);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
