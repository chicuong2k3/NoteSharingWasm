using System.Linq.Dynamic.Core;

namespace SharingNote.Api.Application.Features.Comments.GetComments;


internal sealed class GetCommentsQueryHandler(
    AppDbContext dbContext)
    : IQueryHandler<GetCommentsQuery, GetCommentsResponse>
{
    public async Task<Ardalis.Result.Result<GetCommentsResponse>> Handle(GetCommentsQuery query, CancellationToken cancellationToken)
    {
        var commentsQuery = dbContext.Comments
            .Where(x => x.PostId == query.PostId)
            .Where(x => query.ParentId.HasValue ? x.ParentCommentId == query.ParentId : x.ParentCommentId == null);


        commentsQuery = query.SortDirection?.ToLower() switch
        {
            "asc" => commentsQuery
                        .Where(x => x.PostedDate > query.TimeCursor)
                        .OrderBy(x => x.PostedDate),
            "desc" => commentsQuery
                        .Where(x => x.PostedDate < query.TimeCursor)
                        .OrderByDescending(x => x.PostedDate),
            _ => commentsQuery
                        .Where(x => x.PostedDate < query.TimeCursor)
                        .OrderByDescending(x => x.PostedDate)
        };

        commentsQuery = commentsQuery.Take(query.Count);

        var data = await commentsQuery.ToListAsync(cancellationToken);

        var replyCounts = await dbContext.Comments
            .Where(c => c.ParentCommentId != null && data.Select(d => d.Id).Contains(c.ParentCommentId.Value))
            .GroupBy(c => c.ParentCommentId)
            .Select(g => new { ParentCommentId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(g => g.ParentCommentId!.Value, g => g.Count, cancellationToken);

        var updatedData = data.Select(x => new CommentDto(
                                  x.Id,
                                  x.UserId,
                                  x.PostId,
                                  x.Content,
                                  x.PostedDate,
                                  x.LastModifiedDate,
                                  x.ParentCommentId,
                                  0
                              ))
            .Select(comment =>
        {
            if (replyCounts.TryGetValue(comment.Id, out var count))
            {
                return comment with { ChildrenCount = count };
            }
            return comment;
        }).ToList();

        var remainComments = dbContext.Comments
                                .Where(x => x.PostId == query.PostId)
                                .Where(x => query.ParentId.HasValue ? x.ParentCommentId == query.ParentId : x.ParentCommentId == null);

        DateTime? timeCursor = updatedData.Any() ? query.SortDirection?.ToLower() switch
        {
            "asc" => updatedData.Max(x => x.PostedDate),
            "desc" => updatedData.Min(x => x.PostedDate),
            _ => updatedData.Min(x => x.PostedDate)
        } : null;

        remainComments = query.SortDirection?.ToLower() switch
        {
            "asc" => remainComments
                        .Where(x => x.PostedDate > timeCursor),
            "desc" => remainComments
                        .Where(x => x.PostedDate < timeCursor),
            _ => remainComments
                        .Where(x => x.PostedDate < timeCursor)
        };

        var hasMoreRecords = updatedData.Count == query.Count
            && await remainComments
            .AnyAsync(cancellationToken);

        if (!hasMoreRecords)
        {
            timeCursor = null;
        }

        return Result.Success(new GetCommentsResponse(updatedData, timeCursor));
    }
}
