namespace SharingNote.Api.Application.Features.Posts.CountInteraction;

public sealed record CountInteractionQuery(
    Guid PostId,
    string InteractionType
) : IQuery<int>;
