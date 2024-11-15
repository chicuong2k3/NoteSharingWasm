using SharingNote.Api.Application.Features.Tags.GetTags;

namespace SharingNote.Api.Application.Features.Posts.SearchPosts;

public sealed record PostDto(
    Guid Id,
    string Title,
    string ShortDescription,
    List<TagDto> Tags,
    DateTime PublicationDate,
    Guid UserId
);