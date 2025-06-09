using Babylon.Common.Application.EventBus;
using Babylon.Modules.Channels.Application.Messages.CreateMessage;
using Babylon.Modules.Channels.IntegrationEvents;
using MediatR;

namespace Babylon.Modules.Channels.Presentation.Events;
internal class ChannelPublishMessageIntegrationEventHandler(ISender sender) : IntegrationEventHandler<ChannelPublishMessageIntegrationEvent>
{
    public override async Task Handle(ChannelPublishMessageIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        await sender.Send(new CreateMessageCommand(
            integrationEvent.ChannelId, 
            integrationEvent.UserId, 
            integrationEvent.Message, 
            integrationEvent.PublicationDate, 
            integrationEvent.UserName, 
            integrationEvent.AvatarUrl), cancellationToken);
    }
}
