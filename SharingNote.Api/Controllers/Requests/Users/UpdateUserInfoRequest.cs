namespace SharingNote.Api.Controllers.Requests.Users
{
    public sealed record UpdateUserInfoRequest
    (
        string DisplayName,
        string Avatar
    );
}
