using Babylon.Common.Application.Authorization;
using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Users.Application.Users.GetUserPermissions;
public sealed record GetUserPermissionsQuery(string IdentityId) : IQuery<PermissionsResponse>;
