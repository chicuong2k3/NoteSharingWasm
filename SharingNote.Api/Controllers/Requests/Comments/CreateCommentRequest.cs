namespace SharingNote.Api.Controllers.Requests.Comments
{
    public sealed record CreateCommentRequest
    (
        Guid PostId,
        string Content,
        Guid? ParentId
    );
}
