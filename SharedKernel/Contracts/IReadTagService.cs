namespace SharedKernel.Contracts;

public interface IReadTagService
{
    Task<TagResponse?> GetTagAsync(Guid id);
}

public sealed record TagResponse(Guid Id, string Name, Guid UserId);


