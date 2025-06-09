namespace Babylon.Modules.Channels.Application.Messages.SearchChannelMessages;
public record SearchedChannelMessageDto(Guid MessageChannelId, string UserName, string Message, string Avatar, int Like, int Dislike, DateTime PublicationDate, Guid ChannelId, Guid Id);
