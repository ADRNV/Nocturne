namespace Nocturne.Core.Models
{
    public class Message
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string From { get; set; }

        public string Content { get; set; }
    }
}
