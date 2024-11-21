
namespace SharingNote.Api.Application.Features.Users.RegisterUser
{
    internal sealed class RegisterUserCommandHandler(
        UserManager<AppUser> userManager)
        : ICommandHandler<RegisterUserCommand, RegisterUserResponse>
    {
        public async Task<Ardalis.Result.Result<RegisterUserResponse>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var user = new AppUser
                (
                    command.Email
                );

                var result = await userManager.CreateAsync(user, command.Password);

                if (!result.Succeeded)
                {
                    return Result.Error(new ErrorList(result.Errors.Select(err => err.Description)));
                }

                return Result.Created(new RegisterUserResponse(user.Id, user.Email!));
            }
            catch (Exception ex)
            {
                return Result.CriticalError(ex.Message);
            }

        }
    }
}
