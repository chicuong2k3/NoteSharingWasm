using SharedKernel.Contracts;
using SharingNote.Api.Application.Features.Comments.GetComment;

namespace SharingNote.Api.ContractsImplementation
{
    class CommentService : IReadCommentService
    {
        private readonly ISender _sender;

        public CommentService(ISender sender)
        {
            _sender = sender;
        }
        public async Task<CommentResponse?> GetCommentAsync(Guid id)
        {
            var result = await _sender.Send(new GetCommentQuery(id));

            if (result.IsSuccess)
            {
                return new CommentResponse(
                    result.Value.Id,
                    result.Value.PostId,
                    result.Value.UserId,
                    result.Value.Content,
                    result.Value.PostedDate,
                    result.Value.LastModifiedDate,
                    result.Value.ParentCommentId
                );
            }

            return null;
        }
    }
}
