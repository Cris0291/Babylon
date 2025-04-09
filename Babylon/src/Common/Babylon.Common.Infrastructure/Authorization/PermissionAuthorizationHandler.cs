using Babylon.Common.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Babylon.Common.Infrastructure.Authorization;
internal sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionsRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionsRequirement requirement)
    {
        HashSet<string> permissions = context.User.GetPermissions();

        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
