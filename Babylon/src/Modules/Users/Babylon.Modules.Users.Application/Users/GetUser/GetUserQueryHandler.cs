using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Dapper;

namespace Babylon.Modules.Users.Application.Users.GetUser;
internal sealed class GetUserQueryHandler(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<GetUserQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
            SELECT
               u.id AS {nameof(UserResponse.Id)}
               u.email AS {nameof(UserResponse.Email)}
               u.first_name AS {nameof(UserResponse.FirstName)}
               u.last_name AS {nameof(UserResponse.LastName)}
            FROM users.users u
            WHERE u.id = @Id
            """;

        UserResponse? user = await connection.QuerySingleOrDefaultAsync<UserResponse>(sql, request);

        if (user is null)
        {
            return Result.Failure<UserResponse>(Error.NotFound(description: "User was not found with given id"));
        }

        return user;
    }
}
