namespace SharingNote.Api.Application.Features.Tags
{
    public sealed record TagDto(
        Guid Id,
        string Name,
        Guid UserId
    );
}
