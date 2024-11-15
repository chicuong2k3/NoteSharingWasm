namespace SharingNote.Api.Application.Features.Comments.UpdateComment
{
    internal class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
    {
        public UpdateCommentCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Content).NotEmpty();
        }
    }
}
