using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Members.GetMemberChannels;
public record GetMemberChannelsQuery(Guid MemberId) : IQuery<IEnumerable<int>>;
