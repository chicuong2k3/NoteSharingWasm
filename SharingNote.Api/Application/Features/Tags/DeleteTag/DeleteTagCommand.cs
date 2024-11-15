namespace SharingNote.Api.Application.Features.Tags.DeleteTag;

public record DeleteTagCommand(Guid TagId) : ICommand;
