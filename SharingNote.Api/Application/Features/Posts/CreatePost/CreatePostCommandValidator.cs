namespace SharingNote.Api.Application.Features.Posts.CreatePost
{
    internal class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Content).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
