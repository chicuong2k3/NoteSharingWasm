namespace SharingNote.Api.REST.Controllers.Requests.Posts
{
    public sealed record UpdatePostRequest
    (
        string Title,
        string Content,
        List<Guid> TagIds
    );
}
