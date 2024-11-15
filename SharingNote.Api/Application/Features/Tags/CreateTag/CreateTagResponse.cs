namespace SharingNote.Api.Application.Features.Tags.CreateTag
{
    public sealed record CreateTagResponse
    (
        Guid Id,
        string Name,
        Guid UserId
    );
}
