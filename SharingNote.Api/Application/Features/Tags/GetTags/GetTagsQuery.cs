namespace SharingNote.Api.Application.Features.Tags.GetTags;

public sealed record GetTagsQuery(
) : IQuery<IEnumerable<TagDto>>;
