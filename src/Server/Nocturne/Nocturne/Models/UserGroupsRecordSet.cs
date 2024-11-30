
namespace Nocturne.Models
{
    public class UserGroupsRecordSet : RecordSetResponseBase<CoreGroup>
    {
        public UserGroupsRecordSet(IEnumerable<CoreGroup> records, long totalCount) : base(records, totalCount)
        {
        }
    }
}
