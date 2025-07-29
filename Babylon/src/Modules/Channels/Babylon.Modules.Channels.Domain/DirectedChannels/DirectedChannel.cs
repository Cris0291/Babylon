namespace Babylon.Modules.Channels.Domain.DirectedChannels;

public class DirectedChannel
{
    private DirectedChannel(){}
    public Guid DirectedChannelId { get; private set; }
    public Guid Creator { get; private set; }
    public Guid Participant { get; private set; }

    public static DirectedChannel Create(Guid creator, Guid participant)
    {
         return new DirectedChannel
        {
            DirectedChannelId = Guid.NewGuid(),
            Creator = creator,
            Participant = participant
        };
    }
}
