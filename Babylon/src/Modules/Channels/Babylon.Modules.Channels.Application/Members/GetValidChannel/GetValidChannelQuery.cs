using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Members.GetValidChannel;
public record GetValidChannelQuery(Guid MemberId, Guid ChannnelId) : IQuery<bool>;
