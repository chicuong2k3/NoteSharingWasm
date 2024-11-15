namespace SharingNote.Api.Application.Features.Posts.UpdatePost
{
    internal class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
    {
        public UpdatePostCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Content).NotEmpty();
        }
    }
}
