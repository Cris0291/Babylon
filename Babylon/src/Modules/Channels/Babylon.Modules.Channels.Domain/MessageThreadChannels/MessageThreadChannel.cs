namespace Babylon.Modules.Channels.Domain.MessageThreadChannels;

public sealed class MessageThreadChannel
{
    private MessageThreadChannel() { }
    public Guid MessageThreadChannelId { get; private set; }
    public string UserName { get; private set; }
    public string Message { get; private set; }
    public string AvatarUrl { get; private set; }
    public int NumberOfLikes { get; private set; }
    public int NumberOfDislikes { get; private set; }
    public DateTime PublicationDate { get; private set; }
    public Guid ThreadChannelId { get; private set; }
    public Guid Id { get; private set; }
    public static MessageThreadChannel Create(
        Guid threadId, 
        Guid id, 
        string message, 
        string userName, 
        string avatar,
        DateTime publicationDate = default)
    {
        return new MessageThreadChannel
        {
            ThreadChannelId = threadId,
            Id = id,
            Message = message,
            PublicationDate = publicationDate,
            UserName = userName,
            AvatarUrl = avatar,
            NumberOfLikes = 0,
            NumberOfDislikes = 0,
        };
    }
    public int AddOrRemoveLike(bool like)
    {
        NumberOfLikes = like ? ++NumberOfLikes : --NumberOfLikes;

        return NumberOfLikes;
    }
}
    
