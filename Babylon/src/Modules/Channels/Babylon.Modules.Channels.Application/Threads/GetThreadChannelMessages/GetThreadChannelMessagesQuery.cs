using Babylon.Common.Application.Messaging;
using Babylon.Modules.Channels.Application.Channels.GetChannelMessages;

namespace Babylon.Modules.Channels.Application.Threads.GetThreadChannelMessages;
public sealed record GetThreadChannelMessagesQuery(Guid ThreadId) : IQuery<IEnumerable<MessageResponse>>;
