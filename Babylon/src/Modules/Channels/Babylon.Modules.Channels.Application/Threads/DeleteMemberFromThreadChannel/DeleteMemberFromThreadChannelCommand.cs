using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Threads.DeleteMemberFromThreadChannel;

public sealed record DeleteMemberFromThreadChannelCommand(Guid ThreadChannelId, Guid Id) : ICommand;
