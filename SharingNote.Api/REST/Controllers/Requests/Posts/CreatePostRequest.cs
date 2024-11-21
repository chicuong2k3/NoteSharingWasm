namespace SharingNote.Api.REST.Controllers.Requests.Posts
{
    public sealed record CreatePostRequest
    (
        string Title,
        string Content,
        List<Guid> TagIds
    );
}
