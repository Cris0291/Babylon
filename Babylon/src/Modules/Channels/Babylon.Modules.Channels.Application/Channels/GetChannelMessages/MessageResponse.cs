namespace Babylon.Modules.Channels.Application.Channels.GetChannelMessages;
public sealed record MessageResponse(string UserName, string Message, string Avatar, int NumberOfLikes, int NumberOfDislikes, DateTime PublicationDate, bool? UserLike, bool? UserDislike, bool UserPin, string Emojis);
