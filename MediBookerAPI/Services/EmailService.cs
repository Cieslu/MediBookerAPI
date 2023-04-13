#nullable disable
using System.Net.Mail;
using System.Net;
using MediBookerAPI.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace MediBookerAPI.Services
{
    public class EmailService : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string fromMail = _config["ConnectionEmail:EmailFrom"];
            string fromPassword = _config["ConnectionEmail:EmailPassword"];

            MailMessage mailMessage = new MailMessage();
            mailMessage.Subject = "MediBooker.pl - " + subject;
            mailMessage.Body = "<html><body>" + htmlMessage + "</body></html>";
            mailMessage.IsBodyHtml = true;
            mailMessage.From = new MailAddress(fromMail);
            mailMessage.To.Add(new MailAddress(email));

            SmtpClient client = new SmtpClient
            {
                Port = Convert.ToInt32(_config["ConnectionEmail:Port"]),
                Host = _config["ConnectionEmail:Host"],
                EnableSsl = Convert.ToBoolean(_config["ConnectionEmail:EnableSsl"]),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = Convert.ToBoolean(_config["ConnectionEmail:UseDefaultCredentials"]),
                Credentials = new NetworkCredential(fromMail, fromPassword)

            };

            return client.SendMailAsync(mailMessage);
        }
    }
}
