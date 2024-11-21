namespace SharingNote.Api.REST.Controllers.Requests.Auth;

public record SendEmailRequest
(
    string ToEmail,
    string ToName,
    string Subject,
    string HtmlContent
);