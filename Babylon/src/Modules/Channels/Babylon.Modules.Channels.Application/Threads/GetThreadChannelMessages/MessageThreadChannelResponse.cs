namespace Babylon.Modules.Channels.Application.Threads.GetThreadChannelMessages;

public record MessageThreadChannelResponse(string UserName, string Message, string Avatar, int NumberOfLikes, int NumberOfDislikes, DateTime PublicationDate);
