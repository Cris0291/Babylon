namespace Babylon.Modules.Users.Domain.Users;
public sealed class Permission
{
    public static readonly Permission CreateChannel = new Permission("channels:create");
    public static readonly Permission GetChannels = new Permission("channels:read");
    private Permission() { }
    public Permission(string code)
    {
        Code = code;
    }
    public string Code { get; private set; }
}
