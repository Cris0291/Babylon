namespace Babylon.Modules.Channels.Domain.MessageChannels;
public interface IMessageChannelRepository
{
    Task Insert(MessageChannel messageChannel);
}
