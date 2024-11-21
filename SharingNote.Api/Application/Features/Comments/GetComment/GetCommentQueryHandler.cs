namespace SharingNote.Api.Application.Features.Comments.GetComment;


internal sealed class GetCommentQueryHandler(
    AppDbContext dbContext)
    : IQueryHandler<GetCommentQuery, GetCommentResponse>
{
    public async Task<Ardalis.Result.Result<GetCommentResponse>> Handle(GetCommentQuery query, CancellationToken cancellationToken)
    {
        var comment = await dbContext.Comments
            .Where(x => x.Id == query.Id)
            .FirstOrDefaultAsync();

        if (comment == null)
        {
            return Result.NotFound();
        }

        return Result.Success(comment).Map(x => new GetCommentResponse(
            x.Id,
            x.PostId,
            x.UserId,
            x.Content,
            x.PostedDate,
            x.LastModifiedDate,
            x.ParentCommentId
        ));
    }
}
