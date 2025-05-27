using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Channels.GetChannelStateAccess;
public record GetChannelStateAccessQuery(Guid ChannelId, Guid Id) : IQuery<ChannelAccessStateDto>;
