using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Channels.ChangeChannelType;
public sealed record ChangeChannelTypeCommand(Guid ChannelId, Guid ChannelCreator, string ChannelType) : ICommand;
