using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using SurveyBasket.Options;

namespace SurveyBasket.Services
{
    public class EmailServices(IOptions<EmailSettings> emailsettings, ILogger<EmailServices> logger) : IEmailSender
    {
        private readonly EmailSettings _emailsettings = emailsettings.Value;
        private readonly ILogger<EmailServices> _logger = logger;

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            var message = new MimeKit.MimeMessage
            {
                Sender = MailboxAddress.Parse(_emailsettings.User),
                Subject = subject,
            };

            message.To.Add(MailboxAddress.Parse(email));
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                _logger.LogInformation("Sending email to {email}", email);
                await client.ConnectAsync(_emailsettings.Host, _emailsettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailsettings.User, _emailsettings.Password);
                await client.SendAsync(message);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to send email", ex);
            }
            finally
            {
                await client.DisconnectAsync(true);
            }



        }
    }
}
