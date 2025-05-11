using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Channels.DeleteMemberFormChannel;
public sealed record DeleteMemberFormChannelCommand(Guid ChannelId, Guid Id) : ICommand;

