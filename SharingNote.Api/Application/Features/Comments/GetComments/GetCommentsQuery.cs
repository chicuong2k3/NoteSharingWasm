namespace SharingNote.Api.Application.Features.Comments.GetComments;

public sealed record GetCommentsQuery(
    int Count,
    DateTime TimeCursor,
    Guid PostId,
    Guid? ParentId,
    string? SortDirection
) : IQuery<GetCommentsResponse>;
