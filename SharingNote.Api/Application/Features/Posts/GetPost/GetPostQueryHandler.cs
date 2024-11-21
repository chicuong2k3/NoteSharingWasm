using SharingNote.Api.Application.Features.Tags;

namespace SharingNote.Api.Application.Features.Posts.GetPost;


internal sealed class GetPostQueryHandler(
    AppDbContext dbContext)
    : IQueryHandler<GetPostQuery, PostDto>
{
    public async Task<Ardalis.Result.Result<PostDto>> Handle(GetPostQuery query, CancellationToken cancellationToken)
    {
        var post = await dbContext.Posts
            .Where(x => x.Id == query.Id)
            .Include(x => x.Tags)
            .Include(x => x.Interactions)
            .FirstOrDefaultAsync();

        if (post == null)
        {
            return Result.NotFound();
        }

        return Result.Success(post).Map(x => new PostDto(
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
        ));
    }
}
