using SharedKernel.Contracts;

namespace SharingNote.Wasm.ApiServices;

public interface ICommentService : IReadCommentService
{
    Task<GetCommentsResponse?> GetCommentsAsync(
        Guid postId,
        int count,
        Guid? parentId,
        string? sortDirection,
        DateTime? timeCursor);
    Task<CreateCommentResponse?> CreateCommentAsync(CreateCommentRequest request);
    Task<HttpResponseMessage> UpdateCommentAsync();
    Task<HttpResponseMessage> DeleteCommentAsync(Guid commentId);
}

public record GetCommentsResponse(List<CommentDto> Comments, DateTime? TimeCursor);

public record CommentDto(
    Guid Id,
    Guid UserId,
    Guid PostId,
    string Content,
    DateTime PostedDate,
    DateTime? LastModifiedDate,
    Guid? ParentCommentId,
    int ChildrenCount
);

public record CreateCommentRequest(
    Guid PostId,
    string Content,
    Guid? ParentId);

public record CreateCommentResponse(
    Guid Id,
    Guid UserId,
    Guid PostId,
    string Content,
    DateTime PostedDate,
    DateTime? LastModifiedDate,
    Guid? ParentId
);