namespace Babylon.Modules.Channels.Presentation.Hubs;
public sealed record RemoveUserRequest(Guid ChannelId, Guid AdminId, Guid TargetId);
