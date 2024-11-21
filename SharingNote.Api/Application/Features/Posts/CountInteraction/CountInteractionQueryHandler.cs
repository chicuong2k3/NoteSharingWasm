using SharingNote.Api.Application.Features.Tags;

namespace SharingNote.Api.Application.Features.Posts.CountInteraction;


internal sealed class CountInteractionQueryHandler(
    AppDbContext dbContext)
    : IQueryHandler<CountInteractionQuery, int>
{
    public async Task<Ardalis.Result.Result<int>> Handle(CountInteractionQuery query, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<InteractionType>(query.InteractionType, true, out var interactionType))
        {
            return Result.Error("Invalid interaction type.");
        }

        var interactionCount = await dbContext.PostInteractions
            .Where(x => x.PostId == query.PostId && x.Type == interactionType)
            .CountAsync();


        return Result.Success(interactionCount);
    }
}
