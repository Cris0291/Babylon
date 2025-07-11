﻿using System.Collections.Concurrent;
using Babylon.Modules.Channels.Application.Abstractions.Services;

namespace Babylon.Modules.Channels.Infrastructure.Abstractions.Services;
internal class UserConnectionService : IUserConnectionService
{
    private readonly ConcurrentDictionary<Guid, List<string>> _connections = new();
    public void AddConnection(Guid userId, string connectionId)
    {
        _connections.AddOrUpdate(userId, new List<string> { connectionId},  (_, list) =>
        {
            lock (list)
            {
                list.Add(connectionId);
            }
            return list;
        });
    }

    public List<string> GetConnections(Guid userId)
    {
        return _connections.TryGetValue(userId, out List<string> list) ? list : new List<string>();
    }

    public void RemoveConnection(string connectionId)
    {
        foreach (KeyValuePair<Guid, List<string>> entry in _connections)
        {
            lock (entry.Value)
            {
                if(entry.Value.Remove(connectionId) && entry.Value.Count == 0)
                {
                    _connections.TryRemove(entry.Key, out _);
                }
            }
        }
    }
}
