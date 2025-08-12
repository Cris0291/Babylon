namespace Babylon.Modules.Channels.Domain.Members;

public sealed class Member
{
    private Member() { }
    public Guid MemberId { get; private set; }
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    
    public string Role { get; private set; }
    public ICollection<Member> BlockedMembers { get; private set; } = new List<Member>();
    public ICollection<Member> BlockedByMembers { get; private set; } = new List<Member>();
    public static Member Create(Guid id, string email, string firstName, string lastName)
    {
        return new Member
        {
            MemberId = Guid.NewGuid(),
            Id = id,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
        };
    }
    public bool IsBlockedMember(Guid participant)
    {
        Member? blocked = BlockedMembers.SingleOrDefault(m => m.Id == participant);
        return blocked != null;
    }

    public bool IsBlockedByMember(Guid participant)
    {
        Member? blockedBy = BlockedByMembers.SingleOrDefault(m => m.Id == participant);
        return blockedBy != null;
    }
}
    

