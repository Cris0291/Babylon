using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Users.Application.Users.GetUser;
public sealed record GetUserQuery(Guid Id) : IQuery<UserResponse>;

