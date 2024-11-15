namespace SharingNote.Api.Application.Features.Users.DeleteUser
{
    internal class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
