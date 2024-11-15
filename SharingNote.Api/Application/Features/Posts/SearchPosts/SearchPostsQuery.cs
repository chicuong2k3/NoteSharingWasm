namespace SharingNote.Api.Application.Features.Posts.SearchPosts;

public sealed record SearchPostsQuery(
    string? QueryText,
    string? OrderBy,
    List<Guid>? TagIds,
    Guid? UserId,
    int PageNumber = 1,
    int PageSize = 10
) : IQuery<PagedResult<IEnumerable<PostDto>>>;
