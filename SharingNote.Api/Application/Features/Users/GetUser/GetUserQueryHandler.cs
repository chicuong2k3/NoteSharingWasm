namespace SharingNote.Api.Application.Features.Users.GetUser;


internal sealed class GetUserQueryHandler(
    UserManager<AppUser> userManager)
    : IQueryHandler<GetUserQuery, GetUserResponse>
{
    public async Task<Ardalis.Result.Result<GetUserResponse>> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(query.Id.ToString());

        if (user == null)
        {
            return Result.NotFound();
        }

        return Result.Success(user).Map(x => new GetUserResponse(
            x.Id,
            x.Email!,
            x.DisplayName,
            x.Avatar
        ));
    }
}
