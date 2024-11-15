namespace SharingNote.Api.Application.Features.Comments.CreateComment;

public record CreateCommentCommand(
    Guid PostId,
    Guid UserId,
    string Content,
    Guid? ParentId = null) : ICommand<CreateCommentResponse>;
