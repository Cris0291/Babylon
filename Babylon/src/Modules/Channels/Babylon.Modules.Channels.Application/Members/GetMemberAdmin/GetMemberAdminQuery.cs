using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Members.GetMemberAdmin;
public sealed record GetMemberAdminQuery(Guid ChannelId, Guid Id) : IQuery<bool>;
