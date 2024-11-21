using SharingNote.Api.Application.Features.Tags;
using System.Linq.Dynamic.Core;

namespace SharingNote.Api.Application.Features.Posts.SearchPosts;


internal sealed class SearchPostsQueryHandler(
    AppDbContext dbContext)
    : IQueryHandler<SearchPostsQuery, Ardalis.Result.PagedResult<IEnumerable<PostDto>>>
{
    public async Task<Ardalis.Result.Result<Ardalis.Result.PagedResult<IEnumerable<PostDto>>>> Handle(SearchPostsQuery query, CancellationToken cancellationToken)
    {
        var posts = dbContext.Posts.AsQueryable();

        if (query.UserId.HasValue)
        {
            posts = posts.Where(x => x.UserId == query.UserId);
        }

        if (query.TagIds != null && query.TagIds.Any())
        {
            var tagIds = dbContext.Tags
                .Where(x => query.TagIds.Contains(x.Id))
                .Select(x => x.Id);

            posts = posts
                .Include(x => x.Tags)
                .Where(x => x.Tags.Any(t => tagIds.Contains(t.Id)));
        }

        if (!string.IsNullOrEmpty(query.QueryText))
        {
            posts = posts.Where(x =>
            x.Title.ToLower().Contains(query.QueryText.ToLower())
            || x.Content.ToLower().Contains(query.QueryText.ToLower()));
        }


        var totalItems = await posts.CountAsync(cancellationToken);

        if (!string.IsNullOrEmpty(query.OrderBy))
        {
            try
            {
                posts = posts.OrderBy(query.OrderBy);
            }
            catch (Exception ex)
            {
                return Result.Error("Invalid OrderBy parameter: " + ex.Message);
            }
        }

        posts = posts
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .Include(x => x.Tags);

        var totalPages = (int)Math.Ceiling(totalItems / (double)query.PageSize);

        var data = await posts.ToListAsync(cancellationToken);



        return Result.Success(new Ardalis.Result.PagedResult<IEnumerable<PostDto>>(
            new PagedInfo(
                query.PageNumber,
                query.PageSize,
                totalPages,
                totalItems),
            data.Select(x => new PostDto(
                x.Id,
                x.Title,
                x.ShortDescription,
                x.Tags.Select(t => new TagDto(t.Id, t.Name, t.UserId)).ToList(),
                x.PublicationDate,
                x.UserId,
                x.Interactions.Select(i => new PostInteractionDto(
                    i.Id,
                    i.UserId,
                    i.Type.ToString(),
                    i.CreatedAt
                )).ToList()))

        ));
    }
}
