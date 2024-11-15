namespace SharingNote.Api.Application.Features.Posts.DeletePost
{
    internal class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
    {
        public DeletePostCommandValidator()
        {
            RuleFor(x => x.PostId).NotEmpty();
        }
    }
}
