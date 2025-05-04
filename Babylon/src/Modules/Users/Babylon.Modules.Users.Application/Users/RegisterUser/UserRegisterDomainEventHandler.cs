using Babylon.Common.Application.EventBus;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Users.Application.Users.GetUser;
using Babylon.Modules.Users.Domain.Users;
using Babylon.Modules.Users.IntegrationEvents;
using MediatR;

namespace Babylon.Modules.Users.Application.Users.RegisterUser;
internal class UserRegisterDomainEventHandler(ISender sender, IEventBus bus) : DomainEventHandler<UserRegisterDomainEvent>
{
    public override async Task Handle(UserRegisterDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        Result<UserResponse> result = await sender.Send(new GetUserQuery(domainEvent.UserId), cancellationToken);

        if (!!result.IsSuccess)
        {
            throw new InvalidOperationException("User was not found");
        }

        await bus.PublishAsync(new UserRegisteredIntegrationEvent(
            result.TValue!.Id,
            result.TValue!.Email,
            result.TValue!.FirstName,
            result.TValue!.LastName,
            domainEvent.Id,
            domainEvent.OccurredOnUtc
            ), cancellationToken);
    }
}
