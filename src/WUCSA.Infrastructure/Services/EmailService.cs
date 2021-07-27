using MimeKit;
using MimeKit.Text;
using MailKit.Security;
using MailKit.Net.Smtp;
using WUCSA.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Logging;
using System.Net;

namespace WUCSA.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly IConfiguration _config;

        public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _config = configuration;
        }

        public async Task Send(string to, string subject, string html)
        {
            string smtp_account = _config.GetSection("SMTPConfig:Username").Value;
            string smtp_password = _config.GetSection("SMTPConfig:Password").Value;
            string smtp_reciever = to;

            var from_email = new System.Net.Mail.MailAddress(smtp_account);
            var to_emal = new System.Net.Mail.MailAddress(smtp_reciever);
            var message = new System.Net.Mail.MailMessage(from_email, to_emal);
            message.Subject = subject;
            message.Body = html;

            var smtp = new System.Net.Mail.SmtpClient("smtp.yandex.ru", 465)
            {
                EnableSsl = true,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(smtp_account, smtp_password)
            };

            try
            {
                _logger.LogInformation("Try Send Message");
                await smtp.SendMailAsync(message);
                _logger.LogInformation("Message sent successfully");
            }
            catch (Exception e)
            {
                _logger.LogInformation("Error while sending message:" + e);
               System.Console.WriteLine(e.InnerException.Message);
            }
        }

        public async Task SendAsync(string to, string subject, string html)
        {
            const string SmtpHost = "smtp.yandex.ru";
            const int SmtpPort = 465;
            string SmtpUser = _config.GetSection("SMTPConfig:Username").Value;
            string SmtpPass = _config.GetSection("SMTPConfig:Password").Value;

            try
            {
                // create message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(SmtpUser));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = html };

                _logger.LogInformation("(Async) Try Send Message");
                // send email
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(SmtpHost, SmtpPort, SecureSocketOptions.SslOnConnect);
                await smtp.AuthenticateAsync(SmtpUser, SmtpPass);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
                _logger.LogInformation("(Async) Message sent successfully");
            }
            catch (System.Exception e)
            {
                _logger.LogInformation("(Async) Error while sending message:" + e);
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

        public async Task SendToAllTGAsync(string msg)
        {
            try
            {
                var bot = new Telegram.Bot.TelegramBotClient(_config.GetSection("TelegramAPIToken").Value);
                await bot.SendTextMessageAsync("-1001460153639", msg, ParseMode.Html);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
        }
    }
}