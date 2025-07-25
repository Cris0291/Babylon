using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.DirectedChannels.CreateOrExistDirectedChannel;

public record CreateOrExistDirectedChannelCommand(Guid Creator, Guid Participant) : ICommand<Guid>;
