using SharingNote.Api.Application.Messaging;

namespace SharingNote.Api.Application.Features.Users.RegisterUser;

public record RegisterUserCommand
(
    string Email,
    string Password
) : ICommand<RegisterUserResponse>;