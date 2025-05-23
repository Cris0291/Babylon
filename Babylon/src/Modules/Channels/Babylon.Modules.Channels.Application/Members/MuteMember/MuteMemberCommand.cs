using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Members.MuteMember;
public record MuteMemberCommand(Guid AdminId, Guid Id,Guid ChannelId) : ICommand;
