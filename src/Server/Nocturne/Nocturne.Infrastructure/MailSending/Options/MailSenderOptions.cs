using MailKit.Security;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;

namespace Nocturne.Infrastructure.MailSending.Options
{
    public class MailSenderOptions : IOptions<MailSenderOptions>
    {
        public MailSenderOptions(MailSenderOptions mailSenderOptions)
        {
            Value = mailSenderOptions;
        }

        public MailSenderOptions()
        {
            
        }
        public MailSenderOptions Value { get; private set; }

        [JsonPropertyName("host_adress")]
        public string HostAddress { get; set; }

        [JsonPropertyName("host_port")]
        public int HostPort { get; set; }

        [JsonPropertyName("host_username")]
        public string HostUsername { get; set; }

        [JsonPropertyName("host_password")]
        public string HostPassword { get; set; }

        [JsonPropertyName("host_secure_socket_options")]
        public SecureSocketOptions HostSecureSocketOptions { get; set; }

        [JsonPropertyName("sender_email")]
        public string SenderEmail { get; set; }

        [JsonPropertyName("sender_name")]
        public string SenderName { get; set; }
    }
}
