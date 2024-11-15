namespace SharingNote.Api.Application.Features.Users.ChangePassword;

public record ChangeUserPasswordCommand(
    string Email,
    string NewPassword) : ICommand;
