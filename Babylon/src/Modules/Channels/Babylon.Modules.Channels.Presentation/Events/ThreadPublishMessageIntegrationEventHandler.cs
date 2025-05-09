using Babylon.Common.Application.EventBus;
using Babylon.Modules.Channels.IntegrationEvents;
using MediatR;

namespace Babylon.Modules.Channels.Presentation.Events;
internal sealed class ThreadPublishMessageIntegrationEventHandler(ISender sender) : IntegrationEventHandler<ThreadPublishMessageIntegrationEvent>
{
    public async override Task Handle(ThreadPublishMessageIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        await sender.Send(new ThreadPublishMessageIntegrationEvent(
            integrationEvent.ThreadId, 
            integrationEvent.MemberId, 
            integrationEvent.Message, 
            integrationEvent.PublicationDate, 
            integrationEvent.UserName ,
            integrationEvent.AvatarUrl), 
            cancellationToken);
    }
}
