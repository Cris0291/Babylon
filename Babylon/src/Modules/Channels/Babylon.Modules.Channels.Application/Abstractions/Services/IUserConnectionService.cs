namespace Babylon.Modules.Channels.Application.Abstractions.Services;
public interface IUserConnectionService
{
    void AddConnection(Guid userId, string connectionId);
    void RemoveConnection(string connectionId);
    List<string> GetConnections(Guid userId);
}
