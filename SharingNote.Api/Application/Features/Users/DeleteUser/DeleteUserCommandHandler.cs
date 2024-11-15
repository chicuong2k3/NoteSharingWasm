using SharingNote.Api.Application.Messaging;
using SharingNote.Api.Domain;

namespace SharingNote.Api.Application.Features.Users.DeleteUser
{
    internal class DeleteUserCommandHandler(UserManager<AppUser> userManager)
        : ICommandHandler<DeleteUserCommand>
    {
        public async Task<Result> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(command.UserId.ToString());

            if (user == null)
            {
                return Result.NotFound("User not found.");
            }

            var result = await userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return Result.Error(new ErrorList(result.Errors.Select(err => err.Description)));
            }

            return Result.NoContent();
        }
    }
}
