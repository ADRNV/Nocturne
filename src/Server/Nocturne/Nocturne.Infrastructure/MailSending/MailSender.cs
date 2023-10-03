using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using Nocturne.Core.Mails;
using Nocturne.Core.Mails.Models;
using Nocturne.Infrastructure.MailSending.Options;
using MailKit.Security;

namespace Nocturne.Infrastructure.MailSending
{
    public class MailSender : IMailSender
    {
        private MailSenderOptions _options;

        private ILogger<MailSender> _logger;    

        public MailSender(MailSenderOptions options, ILogger<MailSender> logger)
        {
            _options = options;    

            _logger = logger;
        }

        public async Task<bool> SendMail(MailBase mail)
        {
            using var smtpClient = new SmtpClient();

            bool sendend = false;

            smtpClient.MessageSent += (s, e) => sendend = true;

            await smtpClient.ConnectAsync(_options.HostAddress, _options.HostPort, SecureSocketOptions.SslOnConnect);
            
            await smtpClient.AuthenticateAsync(_options.HostUsername, _options.HostPassword);

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(_options.SenderName, _options.SenderName));

            message.To.Add(new MailboxAddress(mail.To, mail.To));

            message.Subject = mail.Subject;

            var builder = new BodyBuilder();

            builder.TextBody = mail.Content;

            message.Body = builder.ToMessageBody();

            await smtpClient.SendAsync(message);

            await smtpClient.DisconnectAsync(true);
            smtpClient.Dispose();

            _logger.LogInformation(sendend ? $"Message to {message.To} sended succesfully" : $"Faild to send {message.To}");

            return sendend;
        }
    }
}
