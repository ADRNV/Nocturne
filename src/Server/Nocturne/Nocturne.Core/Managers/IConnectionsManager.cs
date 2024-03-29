﻿using Nocturne.Core.Models;

namespace Nocturne.Core.Managers
{
    public interface IConnectionsManager
    {
        Task<string> Connect(User user, string connectionId);

        Task<bool> Disconect(User user, string connectionId);

        Task<bool> Disconect(User user);

        Task<IEnumerable<string>> GetUserConnections(User user);

        Task<bool> IsConnectedUser(User user);
    }
}
