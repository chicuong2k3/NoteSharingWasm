namespace SharingNote.Api.Application.Features.Comments.CreateComment
{
    internal class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator()
        {
            RuleFor(x => x.PostId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Content).NotEmpty();
        }
    }
}
