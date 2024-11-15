using SharingNote.Api.Application.Features.Posts.SearchPosts;
using SharingNote.Api.Application.Features.Tags.GetTags;

namespace SharingNote.Api.Application.Features.Posts.GetPost;


internal sealed class GetPostQueryHandler(
    AppDbContext dbContext)
    : IQueryHandler<GetPostQuery, GetPostResponse>
{
    public async Task<Result<GetPostResponse>> Handle(GetPostQuery query, CancellationToken cancellationToken)
    {
        var post = await dbContext.Posts
            .Where(x => x.Id == query.Id)
            .Include(x => x.Tags)
            .FirstOrDefaultAsync();

        if (post == null)
        {
            return Result.NotFound();
        }

        return Result.Success(post).Map(x => new GetPostResponse(
            x.Id,
            x.Title,
            x.Content,
            x.Tags.Select(t => new TagDto(t.Id, t.Name, t.UserId)).ToList(),
            x.PublicationDate,
            x.UserId
        ));
    }
}
