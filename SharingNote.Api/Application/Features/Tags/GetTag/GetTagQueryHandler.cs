namespace SharingNote.Api.Application.Features.Tags.GetTag;


internal sealed class GetTagQueryHandler(
    AppDbContext dbContext)
    : IQueryHandler<GetTagQuery, TagDto>
{
    public async Task<Ardalis.Result.Result<TagDto>> Handle(GetTagQuery query, CancellationToken cancellationToken)
    {
        var tag = await dbContext.Tags
            .Where(x => x.Id == query.Id)
            .FirstOrDefaultAsync();

        if (tag == null)
        {
            return Result.NotFound();
        }

        return Result.Success(tag).Map(x => new TagDto(
            x.Id,
            x.Name,
            x.UserId
        ));
    }
}
