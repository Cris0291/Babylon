using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.ThreadMessages.PinThreadMessage;

public record PinThreadMessageCommand(Guid ThreadChannelId, Guid ThreadMessageId, Guid Id, bool Pin) : ICommand;
