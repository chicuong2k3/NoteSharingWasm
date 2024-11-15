namespace SharingNote.Api.Application.Features.Tags.GetTags
{
    public sealed record TagDto(
        Guid Id,
        string Name,
        Guid UserId
    );
}
