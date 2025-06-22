using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Threads.ArchiveThread;

public record ArchiveThreadChannelCommand(Guid ThreadChannelId, Guid ChannelId, Guid AdminId) : ICommand;
