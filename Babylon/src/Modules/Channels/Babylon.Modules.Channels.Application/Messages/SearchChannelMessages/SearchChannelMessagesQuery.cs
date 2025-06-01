using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Messages.SearchChannelMessages;
public record SearchChannelMessagesQuery(Guid ChannelId, Guid Id, string Search) : IQuery<IEnumerable<SearchedChannelMessageDto>>;
