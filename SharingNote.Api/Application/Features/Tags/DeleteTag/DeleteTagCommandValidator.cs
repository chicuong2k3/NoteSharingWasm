namespace SharingNote.Api.Application.Features.Tags.DeleteTag
{
    internal class DeleteTagCommandValidator : AbstractValidator<DeleteTagCommand>
    {
        public DeleteTagCommandValidator()
        {
            RuleFor(x => x.TagId).NotEmpty();
        }
    }
}
