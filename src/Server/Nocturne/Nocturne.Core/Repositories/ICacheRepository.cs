namespace Nocturne.Core.Repositories
{
    public interface ICacheRepository<T, C>
    {
        Task<string> Insert(T entity);

        Task Update(T entity);

        Task<T?> Get(string id);

        Task Delete(T entity);

        C Cache { get; }
    }
}
