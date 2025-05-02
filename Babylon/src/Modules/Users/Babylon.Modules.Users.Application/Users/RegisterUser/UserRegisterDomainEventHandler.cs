using Babylon.Common.Application.EventBus;
using Babylon.Common.Application.Messaging;
using Babylon.Modules.Users.Domain.Users;
using MediatR;

namespace Babylon.Modules.Users.Application.Users.RegisterUser;
internal class UserRegisterDomainEventHandler(ISender sender, IEventBus bus) : DomainEventHandler<UserRegisterDomainEvent>
{
    public override async Task Handle(UserRegisterDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        
    }
}
