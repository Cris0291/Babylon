using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Messages.EditMessageChannel;
public record EditMessageChannelCommand(Guid MessageChannelId, Guid ChannelId, Guid Id, string Message) : ICommand;
