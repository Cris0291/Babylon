using Babylon.Common.Application.Messaging;
using Babylon.Modules.Channels.Application.Channels.GetChannelStateAccess;

namespace Babylon.Modules.Channels.Application.Members.GetValidThreadChannel;
public sealed record GetValidThreadChannelQuery(Guid Id, Guid ThreadChannelId, Guid ChannelId) : IQuery<ChannelAccessStateDto>;
