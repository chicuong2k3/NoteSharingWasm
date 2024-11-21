namespace SharingNote.Api.Application.Features.Comments;

public sealed record CommentDto(
    Guid Id,
    Guid UserId,
    Guid PostId,
    string Content,
    DateTime PostedDate,
    DateTime? LastModifiedDate,
    Guid? ParentCommentId,
    int ChildrenCount
);
