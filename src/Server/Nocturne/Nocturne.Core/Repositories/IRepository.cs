namespace Nocturne.Core.Repositories
{
    public interface IRepository<T>
    {
        Task<long> Insert(T person);

        Task<bool> Update(T person);

        Task<bool> Delete(T person);

        Task<IEnumerable<T>> Get();

        Task<T> Get(int id);
    }
}
