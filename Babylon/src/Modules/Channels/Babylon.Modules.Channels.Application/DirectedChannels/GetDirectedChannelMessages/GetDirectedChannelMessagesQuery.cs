using Babylon.Common.Application.Messaging;
using Babylon.Modules.Channels.Application.Channels.GetChannelMessages;

namespace Babylon.Modules.Channels.Application.DirectedChannels.GetDirectedChannelMessages;

public record GetDirectedChannelMessagesQuery(Guid Id, Guid DirectedChannelId) : IQuery<IEnumerable<MessageResponse>>;
