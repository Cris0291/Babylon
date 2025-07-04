using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Channels.GetChannelMessages;
public sealed record GetChannelMessagesQuery(Guid Id, Guid ChannelId) : IQuery<IEnumerable<MessageResponse>>;
