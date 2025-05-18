using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Channels.ArchiveChannel;
public record ArchiveChannelCommand(Guid ChannelId) : ICommand;
