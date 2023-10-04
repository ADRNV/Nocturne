namespace Nocturne.Core.Repositories
{
    public interface IMessagesRepository<T>
    {
        Task<Guid> CreateMessage(Guid userId, T message);

        Task<bool> RemoveMessage(Guid userId, T message);
    }
}
