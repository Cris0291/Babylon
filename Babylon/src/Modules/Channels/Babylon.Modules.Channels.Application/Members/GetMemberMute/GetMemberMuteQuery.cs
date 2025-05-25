using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Members.GetMemberMute;
public record GetMemberMuteQuery(Guid ChannelId, Guid Id) : IQuery<bool>;
