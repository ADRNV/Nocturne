using Nocturne.Core.Models;

namespace Nocturne.Core.Managers
{
    public interface IGroupManager
    {
        Task<bool> AddToGroup(string userName, Group group, CancellationToken cancellationToken);

        Task<bool> AddToGroup(string userName, IEnumerable<Group> groups, CancellationToken cancellationToken);

        Task<bool> RemoveFromGroup(string userName, Group group, CancellationToken cancellationToken);

        Task<bool> RemoveFromGroups(string userName, IEnumerable<Group> groups, CancellationToken cancellationToken);

        Task<Group?> GetGroupById(Guid id);

        Task<Group?> GetGroupByName(Guid id);
    }
}
