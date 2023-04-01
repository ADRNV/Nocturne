namespace Nocturne.Core.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string ImageUrl { get; set; }

        public string UserName { get; set; }

        public string Login { get; set; }

        public string Pasword { get; set; }

        public IList<Group> Groups { get; set; }
    }
}
