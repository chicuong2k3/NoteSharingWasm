namespace SharingNote.Api.Application.Features.Users.UpdateUserInfo
{
    internal class UpdateUserInfoCommandValidator : AbstractValidator<UpdateUserInfoCommand>
    {
        public UpdateUserInfoCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.DisplayName).NotEmpty();
            RuleFor(x => x.Avatar).NotEmpty();
        }
    }
}
