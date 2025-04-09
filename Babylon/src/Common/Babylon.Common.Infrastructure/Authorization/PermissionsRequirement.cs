using Microsoft.AspNetCore.Authorization;

namespace Babylon.Common.Infrastructure.Authorization;
internal sealed class PermissionsRequirement : IAuthorizationRequirement
{
    public PermissionsRequirement(string permission)
    {
        Permission = permission;
    }
    public string Permission { get; private set;  }
}
