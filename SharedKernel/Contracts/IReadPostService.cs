namespace SharedKernel.Contracts;

public interface IReadPostService
{
    Task<PostResponse?> GetPostAsync(Guid id);
}

public record PostResponse
(
    Guid Id,
    string Title,
    string Content,
    List<TagResponse> Tags,
    DateTime PublicationDate,
    Guid UserId,
    List<PostInteractionDto> Interactions
);


public record PostInteractionDto(
    Guid PostId,
    Guid UserId,
    string InteractionType,
    DateTime InteractedAt
);


