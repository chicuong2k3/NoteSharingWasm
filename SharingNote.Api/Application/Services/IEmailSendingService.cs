namespace SharingNote.Api.Application.Services;

public interface IEmailSendingService
{
    Task<bool> SendEmailAsync(string toEmail, string toName, string subject, string htmlContent);
}
