namespace SharingNote.Wasm.ApiServices;

public interface IUserService
{
    Task<HttpResponseMessage> RegisterUserAsync(string email, string password);
    Task<GetUserResponse?> GetUserInfoAsync();
    Task<HttpResponseMessage> UpdateUserAsync(string displayName, string avatar);
    Task<GetUserResponse?> GetUserByIdAsync(Guid id);
    Task<bool> CheckUserExistAsync(string email);
    Task<HttpResponseMessage> SendResetPasswordOtp(string email);
    Task<HttpResponseMessage> ResetPassword(string email, string otp, string newPassword);
}

public sealed record GetUserResponse(
    Guid Id,
    string Email,
    string DisplayName,
    string Avatar
);
