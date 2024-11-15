namespace SharingNote.Api.Application.Features.Comments.GetComment;

public sealed record GetCommentQuery(
    Guid Id
) : IQuery<GetCommentResponse>;
