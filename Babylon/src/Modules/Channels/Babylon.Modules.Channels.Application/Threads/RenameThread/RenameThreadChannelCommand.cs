using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Threads.RenameThread;
public record RenameThreadChannelCommand(Guid ThreadChannelId, string ThreadChannelName, Guid Id) : ICommand;
