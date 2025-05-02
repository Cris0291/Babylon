using Babylon.Common.Domain;

namespace Babylon.Modules.Users.Domain.Users;
public sealed class UserRegisterDomainEvent() : DomainEvent
{
    public Guid UserId { get; init; }
} 
