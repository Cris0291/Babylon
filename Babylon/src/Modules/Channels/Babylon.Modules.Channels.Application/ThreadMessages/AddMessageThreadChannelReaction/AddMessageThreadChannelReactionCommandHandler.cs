using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.MessageThreadChannels;
using Dapper;

namespace Babylon.Modules.Channels.Application.ThreadMessages.AddMessageThreadChannelReaction;

internal sealed class AddMessageThreadChannelReactionCommandHandler(IMessageThreadChannelReactionRepository reactionRepository, IUnitOfWork unitOfWork, IMessageThreadChannelRepository messageThreadChannelRepository) : ICommandHandler<AddMessageThreadChannelReactionCommand>
{
    public async Task<Result> Handle(AddMessageThreadChannelReactionCommand request, CancellationToken cancellationToken)
    {
        MessageThreadChannel? message = await messageThreadChannelRepository.Get(request.ThreadChannelMessageId);

        if (message is null)
        {
            return Result.Failure<int>(Error.Failure(description: "Message was not found"));
        }
        MessageThreadChannelReaction? reaction = await reactionRepository.Get(request.Id, request.ThreadChannelMessageId);

        if (reaction is null)
        {
            var messageReaction = MessageThreadChannelReaction.Create(request.Id, request.ThreadChannelMessageId, request.Emoji);
            await reactionRepository.Insert(messageReaction);
        }
        else
        {
            reaction.AddOrToggleEmoji(request.Emoji);
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
