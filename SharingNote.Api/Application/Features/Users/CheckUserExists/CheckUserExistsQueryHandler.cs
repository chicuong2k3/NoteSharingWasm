using SharingNote.Api.Application.Features.Users.GetUser;

namespace SharingNote.Api.Application.Features.Users.CheckUserExists;


internal sealed class CheckUserExistsQueryHandler(
    UserManager<AppUser> userManager)
    : IQueryHandler<CheckUserExistsQuery, bool>
{
    public async Task<Ardalis.Result.Result<bool>> Handle(CheckUserExistsQuery query, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(query.Email);

        if (user == null)
        {
            return false;
        }

        return true;
    }
}
