using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.Members;
using Babylon.Modules.Channels.Domain.MessageChannels;

namespace Babylon.Modules.Channels.Application.Messages.AddMessageChannelReaction;
internal sealed class AddMessageChannelReactionCommandHandler(
    IMessageChannelRepository repository, 
    IMessageChannelReactionRepository messageChannelReactionRepository ,
    IMemberRepository memberRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddMessageChannelReactionCommand>
{
    public async Task<Result> Handle(AddMessageChannelReactionCommand request, CancellationToken cancellationToken)
    {
        MessageChannel? message = await repository.Get(request.MessageId);

        if (message is null)
        {
            return Result.Failure(Error.Failure(description: "Message was not found"));
        }

        bool isMember = await memberRepository.Exist(request.Id);

        if (!isMember)
        {
            return Result.Failure(Error.Failure(description: "User was not found"));
        }

        MessageChannelReaction? reaction = await messageChannelReactionRepository.Get(request.Id, request.MessageId);

        if (reaction is null)
        {
            var messageReaction = MessageChannelReaction.Create(request.Id, request.MessageId, request.Emoji);
            await messageChannelReactionRepository.Insert(messageReaction);
        }
        else
        {
            reaction.AddOrToggleEmoji(reaction.Emoji);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
