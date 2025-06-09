using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;

namespace Babylon.Modules.Channels.Application.Messages.AddOrRemoveMessageChannelLike;

public sealed record AddOrRemoveMessageChannelLikeCommand(Guid Id, Guid MessageId, Guid ChannelId, bool Like) : ICommand<int>;
