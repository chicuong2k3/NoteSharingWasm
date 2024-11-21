using SharedKernel.Contracts;
using System.ComponentModel.DataAnnotations;

namespace SharingNote.Wasm.ApiServices;

interface IPostService : IReadPostService
{
    Task<CreatePostResponse?> CreatePostAsync(CreatePostRequest request);
    Task<HttpResponseMessage> UpdatePostAsync(Guid id, UpdatePostRequest request);
    Task<HttpResponseMessage> DeletePostAsync(Guid id);

    Task<GetPostsResponse?> GetPostsAsync(
        int pageNumber,
        int pageSize,
        string? queryText,
        string? orderBy,
        List<Guid>? tagIds,
        Guid? userId);

    Task<HttpResponseMessage> InteractPost(InteractionRequest request);
}

record GetPostsResponse(
    PagedInfo PagedInfo,
    List<PostDto> Value
);

record PagedInfo
(
    int PageNumber,
    int PageSize,
    int TotalPages,
    int TotalRecords
);

public record PostDto(
    Guid Id,
    string Title,
    string Content,
    List<TagResponse> Tags,
    DateTime PublicationDate,
    Guid UserId
);


record CreatePostResponse(
    Guid PostId,
    string Title,
    string Content,
    List<TagResponse> Tags,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

class CreatePostRequest
{
    [MaxLength(100, ErrorMessage = "Không được quá 100 ký tự.")]
    [Required(ErrorMessage = "Không được bỏ trống.")]
    public string Title { get; set; } = string.Empty;
    [Required(ErrorMessage = "Không được bỏ trống.")]
    [MinLength(10)]
    public string Content { get; set; } = string.Empty;

    public List<Guid> TagIds { get; set; } = [];
}

class UpdatePostRequest
{
    [MaxLength(100, ErrorMessage = "Không được quá 100 ký tự.")]
    [Required(ErrorMessage = "Không được bỏ trống.")]
    public string Title { get; set; } = string.Empty;
    [Required(ErrorMessage = "Không được bỏ trống.")]
    [MinLength(10)]
    public string Content { get; set; } = string.Empty;

    public List<Guid> TagIds { get; set; } = [];
}

public record InteractionRequest(Guid PostId, Guid UserId, string InteractionType);