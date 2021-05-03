using MimeKit;
using MimeKit.Text;
using MailKit.Security;
using MailKit.Net.Smtp;
using WUCSA.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;

namespace WUCSA.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public void Send(string to, string subject, string html)
        {

            const string SmtpFrom = "sport.wucsa@gmail.com";
            const string SmtpHost = "smtp.gmail.com";
            const int SmtpPort = 465;
            const string SmtpUser = "sport.wucsa@gmail.com";
            string SmtpPass = _config.GetSection("SMTPConfig:Password").Value;

            try
            {
                // create message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(SmtpFrom));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = html };

                // send email
                using var smtp = new SmtpClient();
                smtp.Connect(SmtpHost, SmtpPort, SecureSocketOptions.SslOnConnect);
                smtp.Authenticate(SmtpUser, SmtpPass);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.InnerException.Message);
            }
        }

        public async Task SendAsync(string to, string subject, string html)
        {
            const string SmtpFrom = "sport.wucsa@gmail.com";
            const string SmtpHost = "smtp.gmail.com";
            const int SmtpPort = 465;
            const string SmtpUser = "sport.wucsa@gmail.com";
            string SmtpPass = _config.GetSection("SMTPConfig:Password").Value;

            try
            {
                // create message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(SmtpFrom));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = html };

                // send email
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(SmtpHost, SmtpPort, SecureSocketOptions.SslOnConnect);
                await smtp.AuthenticateAsync(SmtpUser, SmtpPass);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.InnerException.Message);
            }
        }

        public async Task SendTGAsync(string msg)
        {
            try
            {
                var bot = new Telegram.Bot.TelegramBotClient(_config.GetSection("TelegramAPIToken").Value);
                await bot.SendTextMessageAsync("258995364", msg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
        }
    }
}
