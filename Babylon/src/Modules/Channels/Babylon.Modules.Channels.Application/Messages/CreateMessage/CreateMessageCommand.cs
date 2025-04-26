using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Messages.CreateMessage;
public sealed record CreateMessageCommand(Guid ChannelId, Guid MemberId, string Message, DateTime PublicationDate, string UserName, string Avatar) : ICommand;
