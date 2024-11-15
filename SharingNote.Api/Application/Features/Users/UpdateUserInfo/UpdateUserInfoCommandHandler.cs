using SharingNote.Api.Domain;
using SharingNote.Api.Infrastructure.Database.EFCore;

namespace SharingNote.Api.Application.Features.Users.UpdateUserInfo
{
    internal class UpdateUserInfoCommandHandler(
        AppDbContext dbContext,
        UserManager<AppUser> userManager)
        : ICommandHandler<UpdateUserInfoCommand>
    {
        public async Task<Result> Handle(UpdateUserInfoCommand command, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(command.UserId.ToString());

            if (user == null)
            {
                return Result.NotFound("User not found.");
            }

            user.UpdateInfo(command.DisplayName, command.Avatar);
            await dbContext.SaveChangesAsync();

            return Result.NoContent();
        }
    }
}
