namespace Nocturne.Infrastructure.Security.Entities
{
    public class UserGroup
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<User> Users { get; set; }
    }
}
