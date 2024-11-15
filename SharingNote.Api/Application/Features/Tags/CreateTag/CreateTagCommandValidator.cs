namespace SharingNote.Api.Application.Features.Tags.CreateTag
{
    internal class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
    {
        public CreateTagCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
