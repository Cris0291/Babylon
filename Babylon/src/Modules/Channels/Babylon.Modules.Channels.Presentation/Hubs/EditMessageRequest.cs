namespace Babylon.Modules.Channels.Presentation.Hubs;
public record EditMessageRequest(string Message, Guid MessageChannelId, Guid ChannelId, Guid Id);
