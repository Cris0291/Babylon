using Babylon.Common.Domain;

namespace Babylon.Common.Application.Authorization;
public interface IPermissionService
{
    Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId);
}
