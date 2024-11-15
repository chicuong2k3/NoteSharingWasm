namespace SharingNote.Api.Application.Features.Users.ChangePassword
{
    internal class ChangeUserPasswordCommandValidator : AbstractValidator<ChangeUserPasswordCommand>
    {
        public ChangeUserPasswordCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty()
                .EmailAddress();

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(6);

        }
    }
}
