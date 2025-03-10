namespace Babylon.Modules.Channels.Domain.Members;

public sealed class Member
{
    private Member() { }
    public Guid MemberId { get; private set; }
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Role { get; private set; }
}
    

