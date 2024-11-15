namespace SharingNote.Api.Application.Features.Users.GetUser
{
    public sealed record GetUserResponse(
        Guid Id,
        string Email,
        string DisplayName,
        string Avatar
    );
}
