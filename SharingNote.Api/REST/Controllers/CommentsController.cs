using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authentication;
using SharedKernel.Authentication.Extensions;
using SharingNote.Api.Application.Features.Comments.CreateComment;
using SharingNote.Api.Application.Features.Comments.DeleteComment;
using SharingNote.Api.Application.Features.Comments.GetComment;
using SharingNote.Api.Application.Features.Comments.GetComments;
using SharingNote.Api.Application.Features.Comments.UpdateComment;
using SharingNote.Api.REST.Controllers.Requests.Comments;

namespace SharingNote.Api.REST.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly AuthorizationService _authorizationService;

        public CommentsController(
            ISender sender,
            AuthorizationService authorizationService)
        {
            _sender = sender;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CreateCommentResponse>> Create([FromBody] CreateCommentRequest request)
        {
            var command = new CreateCommentCommand(
                request.PostId,
                User.GetUserId(),
                request.Content,
                request.ParentId);
            var response = await _sender.Send(command);
            return this.ToActionResult(response);
        }

        [HttpGet]
        public async Task<ActionResult<GetCommentsResponse>> GetMany([FromQuery] GetCommentsQuery query)
        {
            var response = await _sender.Send(query);
            return this.ToActionResult(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetCommentResponse>> GetOne(Guid id)
        {
            var response = await _sender.Send(new GetCommentQuery(id));
            return this.ToActionResult(response);
        }


        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, UpdateCommentRequest request)
        {
            if (!await _authorizationService.IsAuthorizedToManageComment(User, id))
            {
                return Forbid();
            }

            var command = new UpdateCommentCommand(
                id,
                request.Content);
            var response = await _sender.Send(command);
            return this.ToActionResult(response);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!await _authorizationService.IsAuthorizedToManageComment(User, id))
            {
                return Forbid();
            }

            var response = await _sender.Send(new DeleteCommentCommand(id));
            return this.ToActionResult(response);
        }
    }
}
