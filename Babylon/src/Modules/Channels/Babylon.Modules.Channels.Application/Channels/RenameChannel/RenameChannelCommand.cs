using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Channels.RenameChannel;
public record RenameChannelCommand(Guid ChannelId, string Name, Guid Id) : ICommand;
