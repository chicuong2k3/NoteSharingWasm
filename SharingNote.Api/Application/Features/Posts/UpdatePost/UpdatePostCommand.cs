namespace SharingNote.Api.Application.Features.Posts.UpdatePost;

public record UpdatePostCommand(
    Guid Id,
    string Title,
    string Content,
    List<Guid> TagIds) : ICommand;
