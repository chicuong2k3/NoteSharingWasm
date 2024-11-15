namespace SharingNote.Api.Application.Features.Users.RegisterUser;

public sealed record RegisterUserResponse
(
    Guid UserId,
    string Email
);