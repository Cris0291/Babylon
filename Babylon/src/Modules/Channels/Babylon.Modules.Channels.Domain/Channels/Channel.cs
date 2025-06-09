using Babylon.Common.Domain;


namespace Babylon.Modules.Channels.Domain.Channels;

public sealed class Channel : Entity
{
    private Channel() { }
    public Guid ChannelId { get; private set; }
    public string Name { get; private set; }
    public ChannelType Type { get; private set; }
    public DateTime CreatedAt { get; private set; }
    //Creator id should be user id
    public Guid Creator { get; private set; } 
    public List<Guid> BlockedMembers { get; private set; }
    public List<Guid> Members { get; private set; }
    public static Channel CreateChannel(string channelName, bool publicChannel, Guid creator)
    {
        var channel = new Channel {
            ChannelId = Guid.NewGuid(),
            Name = channelName,
            Type = publicChannel ? ChannelType.Public : ChannelType.Private,
            Creator = creator,
            CreatedAt = DateTime.Now,
            BlockedMembers = []
            };

        channel.RaiseEvent(new AddMemberToChannelDomainEvent(channel.Creator, channel.ChannelId));

        return channel;
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
    public void BlockMember(Guid id)
    {
        BlockedMembers.Add(id);
    }
    public void ArchiveChannel()
    {
        Type = ChannelType.Archived;
    }

    public bool IsArchive()
    {
        return Type == ChannelType.Archived;
    }

    public void Rename(string newName)
    {
        Name = newName;
    }
    public bool IsAdmin(Guid adminId)
    {
        return adminId == Creator;
    }
}

