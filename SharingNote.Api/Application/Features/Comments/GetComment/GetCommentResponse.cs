namespace SharingNote.Api.Application.Features.Comments.GetComment
{
    public sealed record GetCommentResponse(
        Guid Id,
        Guid PostId,
        Guid UserId,
        string Content,
        DateTime PostedDate,
        DateTime? LastModifiedDate,
        Guid? ParentCommentId
    );
}
