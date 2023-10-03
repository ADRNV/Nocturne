namespace Nocturne.Core.Mails.Models
{
    public class ConfirmEmail : MailBase
    {
        public ConfirmEmail(string from, string to) : base(from, to)
        {
            Subject = "Confirm your account on Nocturne";
            Content = "Hello, if you get this mail your regestration in Nocturne almost completed.\n Click link to continue";
        }
    }
}
