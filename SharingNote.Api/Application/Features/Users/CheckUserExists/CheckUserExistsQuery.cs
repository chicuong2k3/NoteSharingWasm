namespace SharingNote.Api.Application.Features.Users.CheckUserExists;

public sealed record CheckUserExistsQuery(string Email) : IQuery<bool>;
