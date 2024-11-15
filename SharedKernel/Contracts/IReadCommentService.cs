namespace SharedKernel.Contracts;

public interface IReadCommentService
{
    Task<CommentResponse?> GetCommentAsync(Guid id);
}

public record CommentResponse
(
    Guid Id,
    Guid PostId,
    Guid UserId,
    string Content,
    DateTime PostedDate,
    DateTime? LastModifiedDate,
    Guid? ParentCommentId
);