namespace SharingNote.Api.REST.Controllers.Requests.Otp
{
    public sealed record SendOtpRequest(string Email, string EmailSubject);
}