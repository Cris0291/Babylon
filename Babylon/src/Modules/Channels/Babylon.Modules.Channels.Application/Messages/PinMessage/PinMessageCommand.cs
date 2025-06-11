using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Messages.PinMessage;
public record PinMessageCommand(Guid ChannelId, Guid MessageId, Guid Id) : ICommand;
