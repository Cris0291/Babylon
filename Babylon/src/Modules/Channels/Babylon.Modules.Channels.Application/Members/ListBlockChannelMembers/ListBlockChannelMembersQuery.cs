using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Members.ListBlockChannelMembers;
public record ListBlockChannelMembersQuery(Guid ChannelId, Guid AdminId) : IQuery<IEnumerable<BlockMemberDto>>;
