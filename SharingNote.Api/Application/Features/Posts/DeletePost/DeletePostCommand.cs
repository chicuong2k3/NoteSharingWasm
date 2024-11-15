namespace SharingNote.Api.Application.Features.Posts.DeletePost;

public record DeletePostCommand(Guid PostId) : ICommand;
