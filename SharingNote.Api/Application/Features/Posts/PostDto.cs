using SharingNote.Api.Application.Features.Tags;

namespace SharingNote.Api.Application.Features.Posts;

public sealed record PostDto(
    Guid Id,
    string Title,
    string Content,
    List<TagDto> Tags,
    DateTime PublicationDate,
    Guid UserId,
    List<PostInteractionDto> Interactions
);