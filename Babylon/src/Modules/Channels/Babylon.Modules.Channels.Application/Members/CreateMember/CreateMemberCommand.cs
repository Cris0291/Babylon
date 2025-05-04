using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Members.CreateMember;
public sealed record CreateMemberCommand(Guid UserId, string Email, string FirstName, string LastName) : ICommand;
