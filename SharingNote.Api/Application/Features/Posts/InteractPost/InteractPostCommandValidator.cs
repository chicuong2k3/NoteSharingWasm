namespace SharingNote.Api.Application.Features.Posts.InteractPost
{
    internal class InteractPostCommandValidator : AbstractValidator<InteractPostCommand>
    {
        public InteractPostCommandValidator()
        {
            RuleFor(x => x.PostId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.InteractionType)
                .Must(value => Enum.TryParse<InteractionType>(value, true, out _))
                .WithMessage("The interaction type must be a valid value of the InteractionType enum.");

        }
    }
}
