using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Messages.PinMessage;

public record PinMessageCommad(Guid ChannelId, Guid MessageId) : ICommand;
