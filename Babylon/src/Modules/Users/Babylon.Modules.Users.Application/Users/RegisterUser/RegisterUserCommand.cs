using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Users.Application.Users.RegisterUser;
public sealed record RegisterUserCommand(string Email, string FirstName, string LastName, string Password) : ICommand<Guid>;

