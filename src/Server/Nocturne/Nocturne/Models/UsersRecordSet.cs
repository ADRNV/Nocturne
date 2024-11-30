using Nocturne.Models;

namespace Nocturne.Models
{
    public class UsersRecordSet : RecordSetResponseBase<CoreUser>
    {
        public UsersRecordSet(IEnumerable<CoreUser> records, long totalCount) : base(records, totalCount)
        {
        }
    }
}
