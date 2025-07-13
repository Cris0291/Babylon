using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Threads.GetThreadsList;

public record GetThreadsListQuery(Guid ChannelId) : IQuery<List<Guid>>;
