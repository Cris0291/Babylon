using System.Threading.Channels;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Domain.Members;
using Babylon.Modules.Channels.Domain.ThreadChannels;

namespace Babylon.Modules.Channels.Domain.Channels;

public sealed class Channel
{
    private Channel() { }
    public Guid ChannelId { get; private set; }
    public string Name { get; private set; }
    public ChannelType Type { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid Creator { get; private set; } 
    public List<Guid> Members { get; private set; }
    public static Channel CreateChannel(string channelName, bool publicChannel, Guid creator)
    {
        return new Channel { Name = channelName, Type = publicChannel ? ChannelType.Public : ChannelType.Private, Creator = creator ,CreatedAt = DateTime.Now};
    }
    public Result ChangeType(string type)
    {
        if (!Enum.TryParse(type, true, out ChannelType result))
        {
            return Result.Failure(Error.Validation(description: "A validation error has occurred. Given channel type does not exist"));
        }
        Type = result;

        return Result.Success();
    }
    public void AddMember(Guid memberId)
    {
        Members.Add(memberId);
    }
}

