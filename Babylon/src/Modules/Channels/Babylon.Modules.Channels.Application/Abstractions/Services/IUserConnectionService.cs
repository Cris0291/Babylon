namespace Babylon.Modules.Channels.Application.Abstractions.Services;
public interface IUserConnectionService
{
    void AddConnection(string userId, string connectionId);
    void RemoveConnection(string connectionId);
    List<string> GetConnections(string userId);
}
