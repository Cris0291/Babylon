namespace Babylon.Modules.Channels.Presentation.Hubs;
internal interface IChannelClient
{
    Task ReceiveMessage(string message);
}
