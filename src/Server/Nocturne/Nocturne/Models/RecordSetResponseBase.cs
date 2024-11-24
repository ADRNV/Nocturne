namespace Nocturne.Models
{
    public class RecordSetResponseBase<T>
    {
        public RecordSetResponseBase(IEnumerable<T> records, long totalCount)
        {
            Records = records;

            TotalCount = totalCount;
        }

        public IEnumerable<T> Records { get; protected set; }

        public long TotalCount { get; protected set; }
    }
}
