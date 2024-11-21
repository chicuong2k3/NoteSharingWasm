using SharingNote.Api.Infrastructure.Database.EFCore;

namespace SharingNote.Api.Application.Features.Tags.GetTags;


internal sealed class GetTagsQueryHandler(
    AppDbContext dbContext)
    : IQueryHandler<GetTagsQuery, IEnumerable<TagDto>>
{
    public async Task<Ardalis.Result.Result<IEnumerable<TagDto>>> Handle(GetTagsQuery query, CancellationToken cancellationToken)
    {
        var tags = await dbContext.Tags.ToListAsync();

        var result = tags.Select(x => new TagDto(x.Id, x.Name, x.UserId));

        return Result.Success(result);
    }
}
