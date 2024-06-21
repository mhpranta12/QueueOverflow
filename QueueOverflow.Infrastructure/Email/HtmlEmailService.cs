using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using QueueOverflow.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Infrastructure.Email
{
    public class HtmlEmailService:IEmailService
    {
        private readonly Smtp _emailSettings;

        public HtmlEmailService(IOptions<Smtp> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public void SendSingleEmail(string receiverName, string receiverEmail,
            string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Queue Overflow Team", "author@queueoverflow.com"));
            message.To.Add(new MailboxAddress("Mehedi ",receiverEmail));
            message.Subject = "Confirmation link '?";

            var builder = new BodyBuilder();
            builder.HtmlBody = body;
            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
                client.Timeout = 30000;
                client.Authenticate(_emailSettings.Username, _emailSettings.Password);

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
