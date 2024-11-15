namespace SharingNote.Api.Application.Features.Comments.DeleteComment;

public record DeleteCommentCommand(Guid Id) : ICommand;
