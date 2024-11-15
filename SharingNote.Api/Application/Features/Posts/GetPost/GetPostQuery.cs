namespace SharingNote.Api.Application.Features.Posts.GetPost;

public sealed record GetPostQuery(
    Guid Id
) : IQuery<GetPostResponse>;
