namespace SharingNote.Api.Application.Features.Users.DeleteUser;

public record DeleteUserCommand(Guid UserId) : ICommand;
