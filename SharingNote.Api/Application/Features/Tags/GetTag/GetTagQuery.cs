namespace SharingNote.Api.Application.Features.Tags.GetTag;

public sealed record GetTagQuery(
    Guid Id
) : IQuery<TagDto>;
