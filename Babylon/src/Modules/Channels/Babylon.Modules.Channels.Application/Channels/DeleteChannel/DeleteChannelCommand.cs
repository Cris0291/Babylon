using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Channels.DeleteChannel;
public record DeleteChannelCommand(Guid ChannelId) : ICommand;
