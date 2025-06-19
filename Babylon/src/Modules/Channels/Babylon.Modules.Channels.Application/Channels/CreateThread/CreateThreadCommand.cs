using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Channels.CreateThread;
public sealed record CreateThreadCommand(string ChannelName, Guid ChannelId, string UserName, string MessageText, DateTime CreationDate, Guid MemberId, string Avatar) : ICommand;

