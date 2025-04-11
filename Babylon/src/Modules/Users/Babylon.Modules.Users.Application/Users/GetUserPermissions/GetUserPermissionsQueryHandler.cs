using System.Data.Common;
using Babylon.Common.Application.Authorization;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Dapper;

namespace Babylon.Modules.Users.Application.Users.GetUserPermissions;
internal sealed class GetUserPermissionsQueryHandler(IDbConnectionFactory dbConnection) : IQueryHandler<GetUserPermissionsQuery, PermissionsResponse>
{
    public async Task<Result<PermissionsResponse>> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnection.OpenConnectionAsync();

        const string sql  = 
            $"""
            SELECT DISTINCT
               u.id AS {nameof(UserPermission.UserId)}
               rp.permission_code AS {nameof(UserPermission.Permission)}
            FROM users.users u
            JOIN users.user_roles ur ON ur.user_id = u.id
            JOIN users.role_permissions rp ON rp.role_name = ur.role_name
            WHERE u.identity_id = @IdentityId
            """;

        List<UserPermission> permissions = (await connection.QueryAsync<UserPermission>(sql, request)).AsList();

        if (!permissions.Any())
        {
            return Result.Failure<PermissionsResponse>(Error.Failure(description: $"User was not foaund with id: {request.IdentityId}"));
        }

        return new PermissionsResponse(permissions[0].UserId, permissions.Select(p => p.Permission).ToHashSet());
    }
    internal sealed class UserPermission
    {
        internal Guid UserId { get; init; }
        internal string Permission { get; init; }
    }
}
