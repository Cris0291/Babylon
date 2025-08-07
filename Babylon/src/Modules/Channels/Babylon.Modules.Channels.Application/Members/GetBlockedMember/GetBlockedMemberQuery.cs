using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Members.GetBlockedMember;

public sealed record GetBlockedMemberQuery(Guid MainMember, Guid ParticipantMember) : IQuery<bool>;
