namespace Babylon.Modules.Channels.Domain.MessageThreadChannels;

public interface IMessageThreadChannelReactionRepository
{
    Task<MessageThreadChannelReaction?> Get(Guid messageThreadChannelId, Guid id);
    Task Insert(MessageThreadChannelReaction reaction);
}
