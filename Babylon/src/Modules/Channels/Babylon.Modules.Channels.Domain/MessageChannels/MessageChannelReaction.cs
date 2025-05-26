using Babylon.Common.Domain;

namespace Babylon.Modules.Channels.Domain.MessageChannels;
public sealed class MessageChannelReaction : Entity
{
    private MessageChannelReaction() { }
    public Guid Id { get; private set; }
    public Guid MessageChannelId { get; private set; }
    public string Emoji { get; private set; }
    public static MessageChannelReaction Create(Guid id, Guid messageId, string emoji)
    {
        return new MessageChannelReaction
        {
            Id = id,
            MessageChannelId = messageId,
            Emoji = emoji
        };
    }
    public void AddOrToggleEmoji(string emoji)
    {
        Emoji = Emoji == emoji ? "" : emoji;
    }
}
