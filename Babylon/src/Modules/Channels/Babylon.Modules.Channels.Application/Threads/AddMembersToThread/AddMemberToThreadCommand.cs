using Babylon.Common.Application.Messaging;
using Babylon.Modules.Channels.Application.Channels.CreateThread;

namespace Babylon.Modules.Channels.Application.Threads.AddMembersToThread;
public record AddMemberToThreadCommand(IEnumerable<MemberDto> members, Guid ThreadId) : ICommand;
