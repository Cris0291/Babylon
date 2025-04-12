namespace Babylon.Modules.Users.Domain.Users;

public sealed class User
{
    private User() { }
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string IdentityId { get; private set; }
    public static User Create(string firstName, string lastName, string email, string identityId)
    {
        return new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            IdentityId = identityId
        };
    }
}
   
