using SharedKernel.Contracts;
using System.ComponentModel.DataAnnotations;

namespace SharingNote.Wasm.ApiServices;

interface ITagService : IReadTagService
{
    Task<IEnumerable<TagResponse>> GetTagsAsync();
    Task<HttpResponseMessage> CreateTagAsync(CreateTagRequest request);
    Task<HttpResponseMessage> DeleteTagAsync(Guid id);
}

class CreateTagRequest
{
    [MaxLength(100, ErrorMessage = "Không được quá 100 ký tự.")]
    [Required(ErrorMessage = "Không được bỏ trống.")]
    public string Name { get; set; } = string.Empty;
}
