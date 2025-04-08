namespace Babylon.Modules.Users.Domain.Users;
public sealed class Role
{
    public static readonly Role Administrator = new Role("Administrator");
    public static readonly Role Member = new Role("Member");
    private Role(string name)
    {
        Name = name;
    }
    private Role() { }
    public string Name { get; private set; }
}
