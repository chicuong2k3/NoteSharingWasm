using HotChocolate.Pagination;

namespace SharingNote.Api.Application.Features.Posts.GetPosts;

public sealed record GetPostsQuery(
    PagingArguments PagingArguments
) : IQuery<Page<PostDto>>;
