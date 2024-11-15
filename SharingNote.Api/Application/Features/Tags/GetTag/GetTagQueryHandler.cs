namespace SharingNote.Api.Application.Features.Tags.GetTag;


internal sealed class GetTagQueryHandler(
    AppDbContext dbContext)
    : IQueryHandler<GetTagQuery, GetTagResponse>
{
    public async Task<Result<GetTagResponse>> Handle(GetTagQuery query, CancellationToken cancellationToken)
    {
        var tag = await dbContext.Tags
            .Where(x => x.Id == query.Id)
            .FirstOrDefaultAsync();

        if (tag == null)
        {
            return Result.NotFound();
        }

        return Result.Success(tag).Map(x => new GetTagResponse(
            x.Id,
            x.Name,
            x.UserId
        ));
    }
}
