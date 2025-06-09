using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Messages.AddMessageChannelReaction;
public sealed record AddMessageChannelReactionCommand(Guid Id, Guid MessageId, Guid ChannelId, string Emoji) : ICommand;
