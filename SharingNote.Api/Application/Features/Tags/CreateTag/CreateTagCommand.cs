namespace SharingNote.Api.Application.Features.Tags.CreateTag;

public record CreateTagCommand(
    string Name,
    Guid UserId) : ICommand<TagDto>;
