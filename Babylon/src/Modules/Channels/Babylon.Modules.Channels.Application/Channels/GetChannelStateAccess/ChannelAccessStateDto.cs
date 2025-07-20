using Babylon.Modules.Channels.Domain.Channels;

namespace Babylon.Modules.Channels.Application.Channels.GetChannelStateAccess;
public record ChannelAccessStateDto(bool IsArchived, bool IsBlocked, bool IsMute, bool ExistChannel, bool IsAuthorized, bool ExistThread);
