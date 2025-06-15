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
using Telegram.Bot;
using Telegram.Bot.Types;

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
            var smtpAccount = _config.GetSection("SMTPConfig:Username").Value;
            var smtpPassword = _config.GetSection("SMTPConfig:Password").Value;

            var fromEmail = new System.Net.Mail.MailAddress(smtpAccount);
            var toEmail = new System.Net.Mail.MailAddress(to);
            var message = new System.Net.Mail.MailMessage(fromEmail, toEmail);
            message.Subject = subject;
            message.Body = html;

            var smtp = new System.Net.Mail.SmtpClient("smtp.yandex.ru", 465)
            {
                EnableSsl = true,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(smtpAccount, smtpPassword)
            };

            try
            {
                _logger.LogInformation("Try Send Message");
                await smtp.SendMailAsync(message);
                _logger.LogInformation("Message sent successfully");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error while sending message: {ex}");
               System.Console.WriteLine(ex.InnerException?.Message);
            }
        }

        public async Task SendAsync(string to, string subject, string html)
        {
            const string smtpHost = "smtp.yandex.ru";
            const int smtpPort = 465;
            var smtpUser = _config.GetSection("SMTPConfig:Username").Value;
            var smtpPass = _config.GetSection("SMTPConfig:Password").Value;

            try
            {
                // create message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(smtpUser));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = html };

                _logger.LogInformation("(Async) Try Send Message");
                // send email
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.SslOnConnect);
                await smtp.AuthenticateAsync(smtpUser, smtpPass);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
                _logger.LogInformation("(Async) Message sent successfully");
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation($"(Async) Error while sending message: {ex}");
                System.Console.WriteLine(ex.InnerException?.Message);
            }
        }

        public async Task SendTGAsync(string msg)
        {
            try
            {
                var tgApiToken = _config.GetSection("TelegramAPIToken").Value;
                if (tgApiToken == null)
                    throw new ArgumentNullException();

                var bot = new Telegram.Bot.TelegramBotClient(tgApiToken);
                var chatId = new ChatId(258995364);
                if (chatId == null)
                    throw new ArgumentNullException();

                await bot.SendMessage(chatId, msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
            }
        }

        public async Task SendToAllTGAsync(string msg)
        {
            try
            {
                var tgApiToken = _config.GetSection("TelegramAPIToken").Value;
                if (tgApiToken == null)
                    throw new ArgumentNullException();

                var bot = new Telegram.Bot.TelegramBotClient(tgApiToken);
                var chatId = new ChatId(-1001460153639);
                if (chatId == null)
                    throw new ArgumentNullException();

                await bot.SendMessage(chatId, msg, ParseMode.Html);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
            }
        }
    }
}