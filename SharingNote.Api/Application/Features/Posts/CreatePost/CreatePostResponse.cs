using SharingNote.Api.Application.Features.Tags.GetTags;

namespace SharingNote.Api.Application.Features.Posts.CreatePost
{
    public sealed record CreatePostResponse
    (
        Guid PostId,
        string Title,
        string Content,
        List<TagDto> Tags,
        DateTime PublicationDate
    );
}
