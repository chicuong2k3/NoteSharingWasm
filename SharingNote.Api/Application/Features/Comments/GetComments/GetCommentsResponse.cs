namespace SharingNote.Api.Application.Features.Comments.GetComments
{
    public record GetCommentsResponse(
        IEnumerable<CommentDto> Comments,
        DateTime? TimeCursor
    );
}
