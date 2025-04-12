using Babylon.Common.Application.Authorization;
using Babylon.Common.Domain;
using Babylon.Modules.Users.Application.Users.GetUserPermissions;
using MediatR;

namespace Babylon.Modules.Users.Infrastructure.Authorization;
internal sealed class PermissionService(ISender sender) : IPermissionService
{
    public async Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId)
    {
        return await sender.Send(new GetUserPermissionsQuery(identityId));
    }
}
