using Microsoft.AspNetCore.Identity;
using Nocturne.Infrastructure.Securiry;
using Nocturne.Infrastructure.Security.Entities;

namespace Nocturne.Infrastructure.Groups
{
    public class GroupsManager : IGroupManager, IDisposable
    {
        private readonly UserManager<User> _userManager;

        private readonly IUserStore<User> _userStore;

        private readonly UsersContext _usersContext;
        
        private bool _disposedValue;

        public GroupsManager(UserManager<User> userManager, IUserStore<User> userStore, UsersContext usersContext)
        {
            _userManager = userManager;

            _userStore = userStore;

            _usersContext = usersContext;
        }

        /// <summary>
        /// Adds user to group
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userGroup"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Added user to group or not</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<bool> AddToGroup(string userName, UserGroup userGroup, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if(user is not null)
            {
                user.UserGroups.Add(userGroup);

                return await SetChange(user, cancellationToken);
            }
            else
            {
                throw new InvalidOperationException($"User with userName {userName} not found");
            }
           
        }

        /// <summary>
        /// Adds user to many group
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userGroup"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Added or not</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<bool> AddToGroup(string userName, IEnumerable<UserGroup> userGroup, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user is not null)
            {
                user.UserGroups.AddRange(userGroup);

                return await SetChange(user, cancellationToken);
            }
            else
            {
                throw new InvalidOperationException($"User with userName {userName} not found");
            }

        }

        /// <summary>
        /// Remove user from group
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userGroup"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Removed or not</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<bool> RemoveFromGroup(string userName, UserGroup userGroup, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user is not null)
            {
                user.UserGroups.Remove(userGroup);

                return await SetChange(user, cancellationToken);
            }
            else
            {
                throw new InvalidOperationException($"User with userName {userName} not found");
            }
        }

        /// <summary>
        /// Remove user from many groups
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userGroups"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Removed or not</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<bool> RemoveFromGroups(string userName, IEnumerable<UserGroup> userGroups, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user is not null)
            {
                user.UserGroups.AddRange(userGroups);

                return await SetChange(user, cancellationToken);
            }
            else
            {
                throw new InvalidOperationException($"User with userName {userName} not found");
            }
        }

        public async Task<UserGroup?> GetGroupById(Guid id)
        {
           return await _usersContext.UsersGroups.FindAsync(new []{ id });
        }

        public async Task<UserGroup?> GetGroupByName(Guid id)
        {
            return await _usersContext.UsersGroups.FindAsync(new[] { id });
        }

        private async Task<bool> SetChange(User user, CancellationToken cancellationToken)
        {
            var identityResult = await _userStore.UpdateAsync(user, cancellationToken);

            return identityResult.Succeeded;
        }


        /// <summary>
        /// Releases resources
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _userManager.Dispose();
                    _userStore.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
