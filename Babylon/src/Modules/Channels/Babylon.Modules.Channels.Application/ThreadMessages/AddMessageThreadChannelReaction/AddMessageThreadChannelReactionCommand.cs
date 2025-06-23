using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.ThreadMessages.AddMessageThreadChannelReaction;

public record AddMessageThreadChannelReactionCommand(Guid Id, Guid ThreadChannelMessageId, Guid ThreadChannelId, string Emoji) : ICommand;
