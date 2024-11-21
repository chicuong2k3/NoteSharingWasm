namespace SharingNote.Api.Application.Features.Posts.CreatePost;

public record CreatePostCommand(
    string Title,
    string Content,
    List<Guid> TagIds,
    Guid UserId) : ICommand<PostDto>;
