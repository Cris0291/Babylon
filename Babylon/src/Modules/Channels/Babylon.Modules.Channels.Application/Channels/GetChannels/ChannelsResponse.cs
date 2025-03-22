using Babylon.Modules.Channels.Domain.Channels;

namespace Babylon.Modules.Channels.Application.Channels.GetChannels;
public sealed record ChannelsResponse(Guid Id, string Name, ChannelType Type);

