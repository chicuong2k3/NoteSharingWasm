using HotChocolate.Pagination;
using SharingNote.Api.Application.Features.Posts.SearchPosts;
using SharingNote.Api.Application.Features.Tags;
using SharingNote.Api.Application.Features.Tags.GetTags;

namespace SharingNote.Api.Application.Features.Posts.GetPosts;


internal sealed class GetPostsQueryHandler(
    AppDbContext dbContext)
    : IQueryHandler<GetPostsQuery, Page<PostDto>>
{
    public async Task<Ardalis.Result.Result<Page<PostDto>>> Handle(GetPostsQuery query, CancellationToken cancellationToken)
    {
        var posts = await dbContext.Posts
                    .OrderBy(x => x.Title)
                    .ThenBy(x => x.Id)
                    .Select(x => new PostDto(
                              x.Id,
                              x.Title,
                              x.Content,
                              x.Tags.Select(t => new TagDto(t.Id, t.Name, t.UserId)).ToList(),
                              x.PublicationDate,
                              x.UserId,
                              x.Interactions.Select(i => new PostInteractionDto(
                                i.Id,
                                i.UserId,
                                i.Type.ToString(),
                                i.CreatedAt
                              )).ToList()
                          ))
                    .ToPageAsync(query.PagingArguments, cancellationToken);

        return posts;
    }
}
