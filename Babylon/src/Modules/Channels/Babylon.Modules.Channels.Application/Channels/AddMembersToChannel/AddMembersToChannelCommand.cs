using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Channels.AddMembersToChannel;
public sealed record AddMembersToChannelCommand(Guid ChannelId, Guid Id) : ICommand;
