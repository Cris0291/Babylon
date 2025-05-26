namespace Babylon.Modules.Channels.Domain.MessageChannels;
public interface IMessageChannelReactionRepository
{
    Task<MessageChannelReaction?> Get(Guid id, Guid messageChannelId);
    Task Insert(MessageChannelReaction reaction);
}
