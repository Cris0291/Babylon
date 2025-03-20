namespace Babylon.Modules.Channels.Domain.Members;
public interface IMemberRepository
{
    Task<bool> Exist(Guid MemberId);
}
