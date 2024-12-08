namespace Nocturne.Core.Filtering
{
    public interface ISearchMapper
    {
        public Type Type { get; }

        public IEnumerable<ISearch> BuildSearch(string[] rawSearchCriterias);
    }
}
