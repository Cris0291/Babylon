using Babylon.Common.Domain;

namespace Babylon.Modules.Channels.Domain.MessageChannels;
public sealed class MessageChannelReaction : Entity
{
    private MessageChannelReaction() { }
    public Guid Id { get; private set; }
    public Guid MessageChannelId { get; private set; }
    public string? Emoji { get; private set; }
    public bool? Like { get; private set; }
    public bool? Dislike { get; private set; }
    public static MessageChannelReaction Create(Guid id, Guid messageId, string? emoji = null, bool? like = null, bool? dislike = null)
    {
        return new MessageChannelReaction
        {
            Id = id,
            MessageChannelId = messageId,
            Emoji = emoji,
            Like = like,
            Dislike = dislike,
        };
    }
    public void AddOrToggleEmoji(string emoji)
    {
        Emoji = Emoji == emoji ? "" : emoji;
    }

    public Result<int> AddOrRemoveLike(bool like)
    {
        if (Like != null && Like == like)
        {
            return Result.Failure<int>(Error.Failure(description: "Something unexpected happened. Like was added or remove twice"));
        }

        Like = like;

        return Result.Success<int>(0);
    }
}
