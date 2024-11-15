namespace SharingNote.Api.Application.Features.Comments.UpdateComment;

public record UpdateCommentCommand(
    Guid Id,
    string Content) : ICommand;
