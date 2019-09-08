using System.Threading.Tasks;
using IoTPlatform.Website.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace IoTPlatform.Website.Services
{
    public class SendGridEmailService : IEmailSender
    {
        private readonly SendGridSettings _settings;

        public SendGridEmailService(IOptions<SendGridSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var apiKey = _settings.ApiKey;

            var myMessage = new SendGridMessage();
            myMessage.AddTo(email);
            myMessage.From = new EmailAddress(_settings.FromEmail, _settings.FromName);
            myMessage.Subject = subject;
            myMessage.HtmlContent = htmlMessage;

            var transportWeb = new SendGridClient(apiKey);
            await transportWeb.SendEmailAsync(myMessage);
        }
    }
}