using Babylon.Common.Application.EventBus;
using Babylon.Modules.Channels.Application.Members.CreateMember;
using Babylon.Modules.Users.IntegrationEvents;
using MediatR;

namespace Babylon.Modules.Channels.Presentation.Members;
internal sealed class UserRegisteredIntegrationEventHandler(ISender sender) : IntegrationEventHandler<UserRegisteredIntegrationEvent>
{
    public override async Task Handle(UserRegisteredIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        await sender.Send(new CreateMemberCommand(integrationEvent.UserId, integrationEvent.Email, integrationEvent.FirstName, integrationEvent.LastName), cancellationToken);
    }
}
