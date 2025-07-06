using Babylon.Modules.Channels.Domain.Channels;

namespace Babylon.Modules.Channels.Application.Channels.GetChannelStateAccess;
public record ChannelAccessStateDto(ChannelType Type, bool IsBlocked, bool IsMute);
