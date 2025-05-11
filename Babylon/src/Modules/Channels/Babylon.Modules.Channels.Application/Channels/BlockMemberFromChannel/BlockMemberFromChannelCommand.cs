using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Channels.BlockMemberFromChannel;
public sealed record BlockMemberFromChannelCommand(Guid ChannelId, Guid Id) : ICommand;
