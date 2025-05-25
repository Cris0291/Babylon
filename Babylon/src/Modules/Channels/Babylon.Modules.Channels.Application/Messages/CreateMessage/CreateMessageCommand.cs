using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Messages.CreateMessage;
public sealed record CreateMessageCommand(Guid ChannelId, Guid Id, string Message, DateTime PublicationDate, string UserName, string Avatar) : ICommand;
