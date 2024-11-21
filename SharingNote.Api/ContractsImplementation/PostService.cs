using SharedKernel.Contracts;
using SharingNote.Api.Application.Features.Posts.GetPost;

namespace SharingNote.Api.ContractsImplementation
{
    class PostService : IReadPostService
    {
        private readonly ISender _sender;

        public PostService(ISender sender)
        {
            _sender = sender;
        }
        public async Task<PostResponse?> GetPostAsync(Guid id)
        {
            var result = await _sender.Send(new GetPostQuery(id));

            if (result.IsSuccess)
            {
                return new PostResponse(
                    result.Value.Id,
                    result.Value.Title,
                    result.Value.Content,
                    result.Value.Tags.Select(t => new TagResponse(t.Id, t.Name, t.UserId)).ToList(),
                    result.Value.PublicationDate,
                    result.Value.UserId,
                    result.Value.Interactions.Select(i => new PostInteractionDto(
                        i.PostId,
                        i.UserId,
                        i.InteractionType,
                        i.InteractedAt)).ToList()
                );
            }

            return null;
        }
    }
}
