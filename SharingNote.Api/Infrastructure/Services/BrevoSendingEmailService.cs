using Microsoft.Extensions.Options;
using SharingNote.Api.Application.Services;
using System.Net.Http.Headers;

namespace SharingNote.Api.Infrastructure.Services
{
    public class BrevoSendingEmailService : IEmailSendingService
    {
        private readonly EmailSettings _emailSettings;
        public BrevoSendingEmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }
        public async Task<bool> SendEmailAsync(string toEmail, string toName, string subject, string htmlContent)
        {
            var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.brevo.com/v3/smtp/email");
            request.Headers.Add("api-key", _emailSettings.ApiKey);

            var emailData = new
            {
                sender = new
                {
                    name = _emailSettings.SenderName,
                    email = _emailSettings.SenderEmail
                },
                to = new[] { new { email = toEmail, name = toName } },
                subject = subject,
                htmlContent = htmlContent
            };

            request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(emailData), System.Text.Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }
    }
}
