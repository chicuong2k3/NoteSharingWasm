namespace SharingNote.Wasm.ApiServices;

public interface IEmailService
{
    Task<HttpResponseMessage> SendEmailAsync(EmailRequest request);
}

public record EmailRequest(string ToEmail, string ToName, string Subject, string HtmlContent);