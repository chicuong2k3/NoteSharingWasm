namespace SharingNote.Api.Application.Features.Users.UpdateUserInfo;

public record UpdateUserInfoCommand(
    Guid UserId,
    string DisplayName,
    string Avatar
    ) : ICommand;
