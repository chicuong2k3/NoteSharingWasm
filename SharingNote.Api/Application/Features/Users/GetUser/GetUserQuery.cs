namespace SharingNote.Api.Application.Features.Users.GetUser;

public sealed record GetUserQuery(
    Guid Id
) : IQuery<GetUserResponse>;
