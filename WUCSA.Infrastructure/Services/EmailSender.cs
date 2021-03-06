using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WUCSA.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace WUCSA.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            const string username = "sport.wucsa@gmail.com";
            var password = _config.GetSection("EmailPassword").Value;

            using var mailMessage = new MailMessage(username, email)
            {
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            using var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(username, password)
            };

            // ReSharper disable once AsyncConverter.AsyncAwaitMayBeElidedHighlighting
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
