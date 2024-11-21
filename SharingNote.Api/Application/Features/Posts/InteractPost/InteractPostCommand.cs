namespace SharingNote.Api.Application.Features.Posts.InteractPost;

public record InteractPostCommand(
    Guid PostId, 
    Guid UserId, 
    string InteractionType) : ICommand;
