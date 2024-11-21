namespace SharingNote.Api.Domain;

public class PostInteraction
{
    public PostInteraction(
        Guid postId,
        Guid userId,
        InteractionType type)
    {
        Id = Guid.NewGuid();
        PostId = postId;
        UserId = userId;
        Type = type;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid PostId { get; private set; }
    public Guid UserId { get; private set; }
    public InteractionType Type { get; private set; }
    public DateTime CreatedAt { get; private set; }
}
