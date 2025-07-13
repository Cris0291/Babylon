using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.ThreadMessages.EditThreadMessages;

public record EditThreadMessageQuery(Guid MessageThreadChannelId, string Message) : ICommand;
