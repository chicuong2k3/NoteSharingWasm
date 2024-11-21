namespace SharingNote.Api.Application.Features.Posts
{
    public sealed record PostInteractionDto
    (
        Guid PostId,
        Guid UserId,
        string InteractionType,
        DateTime InteractedAt
    );
}
