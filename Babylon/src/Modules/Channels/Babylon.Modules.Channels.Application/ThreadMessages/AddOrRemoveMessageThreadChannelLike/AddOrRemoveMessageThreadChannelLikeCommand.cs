using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.ThreadMessages.AddOrRemoveMessageThreadChannelLike;

public record AddOrRemoveMessageThreadChannelLikeCommand(Guid Id, Guid ThreadChannelMessageId, Guid ThreadChannelId, bool Like) : ICommand<int>;
