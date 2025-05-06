using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.ThreadMessages.CreateThreadMessage;
public sealed record CreateThreadMessageCommand(Guid ThreadId, Guid MemberId, string Message, DateTime PublicationDate, string UserName, string Avatar) : ICommand;
