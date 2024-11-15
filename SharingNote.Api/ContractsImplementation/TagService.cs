using SharedKernel.Contracts;
using SharingNote.Api.Application.Features.Posts.GetPost;
using SharingNote.Api.Application.Features.Tags.GetTag;

namespace SharingNote.Api.ContractsImplementation
{
    class TagService : IReadTagService
    {
        private readonly ISender _sender;

        public TagService(ISender sender)
        {
            _sender = sender;
        }
        public async Task<TagResponse?> GetTagAsync(Guid id)
        {
            var result = await _sender.Send(new GetTagQuery(id));

            if (result.IsSuccess)
            {
                return new TagResponse(result.Value.Id, result.Value.Name, result.Value.UserId);
            }

            return null;
        }
    }
}
