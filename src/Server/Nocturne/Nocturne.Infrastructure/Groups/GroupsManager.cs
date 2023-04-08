using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Nocturne.Core.Managers;
using Nocturne.Core.Models;
using Nocturne.Infrastructure.Securiry;
using Nocturne.Infrastructure.Security.Entities;
using User = Nocturne.Infrastructure.Security.Entities.User;

namespace Nocturne.Infrastructure.Groups
{
    public class GroupsManager : IGroupManager, IDisposable
    {
        private readonly UserManager<User> _userManager;

        private readonly IUserStore<User> _userStore;

        private readonly UsersContext _usersContext;

        private readonly IMapper _mapper;
        
        private bool _disposedValue;

        public GroupsManager(UserManager<User> userManager, IUserStore<User> userStore, UsersContext usersContext, IMapper mapper)
        {
            _userManager = userManager;

            _userStore = userStore;

            _usersContext = usersContext;

            _mapper = mapper;
        }

        /// <summary>
        /// Adds user to group
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userGroup"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Added user to group or not</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<bool> AddToGroup(string userName, Group group, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if(user is not null)
            {
                var userGroup = _mapper.Map<Group, UserGroup>(group);

                user.UserGroups!.Add(userGroup);

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
        public async Task<bool> AddToGroup(string userName, IEnumerable<Group> groups, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user is not null)
            {
                var userGroups = _mapper
                    .Map<IEnumerable<Group>, IEnumerable<UserGroup>>(groups);

                user.UserGroups!.AddRange(userGroups);

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
        public async Task<bool> RemoveFromGroup(string userName, Group group, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user is not null)
            {
                var userGroup = _mapper.Map<Group, UserGroup>(group);

                user.UserGroups!.Remove(userGroup);

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
        public async Task<bool> RemoveFromGroups(string userName, IEnumerable<Group> groups, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user is not null)
            {
                var userGroups = _mapper
                    .Map<IEnumerable<Group>, IEnumerable<UserGroup>>(groups);

                user.UserGroups!.AddRange(userGroups);

                return await SetChange(user, cancellationToken);
            }
            else
            {
                throw new InvalidOperationException($"User with userName {userName} not found");
            }
        }

        public async Task<Group?> GetGroupById(Guid id)
        {
           return _mapper
                .Map<UserGroup?, Group>(await _usersContext.UsersGroups.FindAsync(new []{ id }));
        }

        public async Task<Group?> GetGroupByName(Guid id)
        {
            return _mapper
                .Map<UserGroup?, Group>(await _usersContext.UsersGroups.FindAsync(new[] { id }));
        }

        private async Task<bool> SetChange(User user, CancellationToken cancellationToken)
        {
            var identityResult = await _userStore.UpdateAsync(user, cancellationToken);

            return identityResult.Succeeded;
        }

        public async Task<bool> UserInGroup(Core.Models.User user, Group group)
        {
            var userGroup = _mapper.Map<UserGroup>(group);

            var identityUser = _mapper.Map<User>(user);

            if(identityUser is not null)
            {
                return await Task.Run(() => identityUser.UserGroups.Contains(userGroup));
            }
            else
            {
                throw new InvalidOperationException($"User with userName {user.UserName} not found");
            }

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
