namespace SharingNote.Api.REST.Controllers.Requests.Users;

public sealed record ResetPasswordRequest
(
    string Email,
    string Otp,
    string NewPassword
);
