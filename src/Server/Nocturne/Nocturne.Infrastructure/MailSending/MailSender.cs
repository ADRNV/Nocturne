using Castle.Core.Configuration;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Nocturne.Core.Mails.Models;
using Nocturne.Infrastructure.MailSending.Options;

namespace Nocturne.Infrastructure.MailSending
{
    public class MailSender
    {
        private IOptions<MailSenderOptions> _options;

        private ILogger<MailSender> _logger;    

        public MailSender(IOptions<MailSenderOptions> options, ILogger<MailSender> logger)
        {
            _options = options;    

            _logger = logger;
        }

        public async Task<bool> SendMail(MailBase mail)
        {
            using var smtpClient = new SmtpClient();

            bool sendend = false;

            smtpClient.MessageSent += (s, e) => sendend = true;

            smtpClient.Connect(_options.Value.HostAddress, _options.Value.HostPort, _options.Value.HostSecureSocketOptions);

            await smtpClient.AuthenticateAsync(_options.Value.HostUsername, _options.Value.HostPassword);

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(_options.Value.SenderName, _options.Value.SenderName));

            message.To.Add(new MailboxAddress(mail.To, mail.To));

            message.Subject = mail.Subject;

            var builder = new BodyBuilder();

            builder.TextBody = mail.Content;

            message.Body = builder.ToMessageBody();

            await smtpClient.SendAsync(message);

            _logger.LogInformation(sendend ? $"Message to {message.To} sended succesfully" : $"Faild to send {message.To}");

            return sendend;
        }
    }
}
