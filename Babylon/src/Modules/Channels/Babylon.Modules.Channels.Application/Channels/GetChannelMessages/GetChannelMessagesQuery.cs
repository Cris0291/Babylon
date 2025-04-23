using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Channels.GetChannelMessages;
public sealed record GetChannelMessagesQuery(Guid Id) : IQuery<IEnumerable<MessageResponse>>;
