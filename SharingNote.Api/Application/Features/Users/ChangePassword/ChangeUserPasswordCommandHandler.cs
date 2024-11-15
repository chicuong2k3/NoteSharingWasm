using SharingNote.Api.Domain;

namespace SharingNote.Api.Application.Features.Users.ChangePassword
{
    internal class ChangeUserPasswordCommandHandler(UserManager<AppUser> userManager)
        : ICommandHandler<ChangeUserPasswordCommand>
    {
        public async Task<Result> Handle(ChangeUserPasswordCommand command, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(command.Email);

            if (user == null)
            {
                return Result.NotFound("User not found.");
            }

            var result = await userManager.RemovePasswordAsync(user);

            if (!result.Succeeded)
            {
                return Result.Error(new ErrorList(result.Errors.Select(err => err.Description)));
            }

            result = await userManager.AddPasswordAsync(user, command.NewPassword);

            if (!result.Succeeded)
            {
                return Result.Error(new ErrorList(result.Errors.Select(err => err.Description)));
            }

            return Result.NoContent();
        }
    }
}
