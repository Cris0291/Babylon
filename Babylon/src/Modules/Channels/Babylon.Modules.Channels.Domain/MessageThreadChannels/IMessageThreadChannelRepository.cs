namespace Babylon.Modules.Channels.Domain.MessageThreadChannels;
public interface IMessageThreadChannelRepository
{
    Task Insert(MessageThreadChannel messageThreadChannel);
    Task<MessageThreadChannel?> Get(Guid messageThreadChannelId);
}
