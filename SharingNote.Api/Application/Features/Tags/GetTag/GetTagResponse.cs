namespace SharingNote.Api.Application.Features.Tags.GetTag
{
    public sealed record GetTagResponse(
        Guid Id,
        string Name,
        Guid UserId
    );
}
