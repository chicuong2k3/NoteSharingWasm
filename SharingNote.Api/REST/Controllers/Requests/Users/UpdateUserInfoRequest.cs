namespace SharingNote.Api.REST.Controllers.Requests.Users
{
    public sealed record UpdateUserInfoRequest
    (
        string DisplayName,
        string Avatar
    );
}
