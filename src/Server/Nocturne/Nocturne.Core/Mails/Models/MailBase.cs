namespace Nocturne.Core.Mails.Models
{
    public class MailBase
    {
        public MailBase(string from, string to)
        {

        }

        public string From { get; set; }

        public string To { get; set; }

        public string Content { get; set; }

        public string Subject { get; set; }
    }
}
