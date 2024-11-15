namespace SharingNote.Api.Application.Features.Comments.CreateComment
{
    public sealed record CreateCommentResponse
    (
        Guid Id,
        Guid PostId,
        Guid UserId,
        string Content,
        DateTime PostedDate,
        DateTime? LastModifiedDate,
        Guid? ParentId
    );
}
