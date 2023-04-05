namespace Nocturne.Core.Models
{
    public class Group
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<User> Users { get; set; }
    }
}
