using HotChocolate.Pagination;
using HotChocolate.Types.Pagination;
using SharingNote.Api.Application.Features.Posts;
using SharingNote.Api.Application.Features.Posts.GetPosts;

namespace SharingNote.Api.GraphQL
{
    [QueryType]
    public static class PostQueries
    {
        [UsePaging]
        public static async Task<Connection<PostDto>> GetPostsAsync(
            PagingArguments pagingArguments,
            ISender sender)
        {
            var result = await sender.Send(new GetPostsQuery(pagingArguments));

            if (result.IsSuccess)
            {
                return result.Value.ToConnection();
            }

            return (Connection<PostDto>)Connection.Empty();
        }
    }
}
