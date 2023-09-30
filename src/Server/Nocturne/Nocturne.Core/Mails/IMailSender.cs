using Nocturne.Core.Mails.Models;

namespace Nocturne.Core.Mails
{
    public interface IMailSender
    {
        Task<bool> SendMail(MailBase mail);
    }
}
