namespace Babylon.Modules.Users.Domain.Users;
public interface IUserRepository
{
    Task Insert(User user);
}
