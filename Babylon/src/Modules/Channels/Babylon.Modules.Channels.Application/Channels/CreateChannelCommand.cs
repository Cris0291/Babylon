using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Channels;
public sealed record CreateChannelCommand(string ChannelName, bool IsPublicChannel, Guid MemberId) : ICommand<Guid>;
