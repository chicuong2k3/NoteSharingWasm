using SharingNote.Api.Application.Features.Tags.GetTags;

namespace SharingNote.Api.Application.Features.Posts.GetPost
{
    public sealed record GetPostResponse(
        Guid Id,
        string Title,
        string Content,
        List<TagDto> Tags,
        DateTime PublicationDate,
        Guid UserId
    );
}
