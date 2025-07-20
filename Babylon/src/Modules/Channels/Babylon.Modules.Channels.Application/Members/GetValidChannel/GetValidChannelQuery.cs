using Babylon.Common.Application.Messaging;
using Babylon.Modules.Channels.Application.Channels.GetChannelStateAccess;

namespace Babylon.Modules.Channels.Application.Members.GetValidChannel;
public record GetValidChannelQuery(Guid Id, Guid ChannelId) : IQuery<ChannelAccessStateDto>;
