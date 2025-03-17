using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Channels;
public sealed record CreateChannelCommand() : ICommand<Guid>;
