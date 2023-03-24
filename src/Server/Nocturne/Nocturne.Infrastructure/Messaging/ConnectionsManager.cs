using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Nocturne.Core.Managers;
using Nocturne.Infrastructure.Caching;
using Nocturne.Infrastructure.Security.Entities;

namespace Nocturne.Infrastructure.Messaging
{
    /// <summary>
    /// Containts mapping between connections and users, managing connections.
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class ConnectionsManager<TUser> : IConnectionsManager where TUser: IdentityUser<Guid>
    {
        private readonly IRedisCacheRepository<Connection> _cacheRepository;

        private UserManager<TUser> _userManager;

        /// <summary>
        /// Creaates instamce of <see cref="ConnectionsManager{TUser}"/>
        /// </summary>
        /// <param name="cacheRepository">Repository to chaching connections</param>
        /// <param name="userManager">Manager for mapping connections and users</param>
        public ConnectionsManager(IRedisCacheRepository<Connection> cacheRepository, UserManager<TUser> userManager)
        {
            _cacheRepository = cacheRepository;

            _userManager = userManager;
        }

        /// <summary>
        /// Adds connection and user pair to db
        /// </summary>
        /// <param name="user">User who connecting</param>
        /// <param name="connection">Connection id</param>
        /// <returns>Id database</returns>
        public async Task<string> Connect(Core.Models.User user, string connectionId)
        {
            var chatUser = await _userManager.FindByNameAsync(user.UserName);

            return await _cacheRepository.Insert(new Connection
            {
                UserId = chatUser.Id,
                ConnectionId = connectionId,
                ConnectedAt = DateTime.Now
            });
        }

        /// <summary>
        /// Close connection for selected user
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="connectionId">Id of connection</param>
        /// <returns>Date of disconect</returns>
        public async Task<bool> Disconect(Core.Models.User user, string connectionId)
        {
            var disconectingUser = await _userManager.FindByNameAsync(user.UserName);

            if (disconectingUser is null)
            {
                throw new InvalidOperationException($"User with '{user.UserName}' are not exists");
            }

            var connection = _cacheRepository.Cache
                    .Where(c => c.ConnectionId == connectionId && c.UserId == disconectingUser.Id)
                    .First();

            if (connection is not null)
            {
                await _cacheRepository.Delete(connection);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Closes all connections for user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Date of diconect</returns>
        public async Task<bool> Disconect(Core.Models.User user)
        {
            
            var disconectingUser = await _userManager.FindByNameAsync(user.UserName);
            
            if (disconectingUser is null) 
            {
                throw new InvalidOperationException($"User with '{user.UserName}' are not exists");
            }

            var connections = _cacheRepository.Cache
                  .Where(c => c.UserId == disconectingUser.Id)
                  .AsEnumerable();

            if (connections.Any())
            {
                await _cacheRepository.Cache.DeleteAsync(connections);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets all user connections
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Collection of connections</returns>
        public async Task<IEnumerable<string>> GetUserConnections(Core.Models.User user)
        {
            var connectedUser = await _userManager.FindByNameAsync(user.UserName);

            if (connectedUser is not null)
            {
                return _cacheRepository.Cache.Where(c => c.UserId == user.Id)
                    .Select(c => c.ConnectionId)
                    .AsEnumerable();
            }
            else
            {
                throw new InvalidOperationException($"User with '{user.UserName}' are not exists");
            }
        }

        /// <summary>
        /// Checks have a user at least one connection
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>If have least one connection - <see langword="true"/> else <see langword="false"/></returns>
        public async Task<bool> IsConnectedUser(Core.Models.User user)
        {
            var connectedUser = await _userManager.FindByNameAsync(user.UserName);

            return _cacheRepository.Cache
                .Where(c => c.UserId == connectedUser.Id)
                .IsNullOrEmpty();
        }
    }
}
