namespace SharingNote.Api.Application.Features.Comments.DeleteComment
{
    internal class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>
    {
        public DeleteCommentCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
