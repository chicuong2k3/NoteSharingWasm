using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SharedKernel.Authentication;
using SharedKernel.Authentication.Extensions;
using SharingNote.Api.Application.Features.Posts;
using SharingNote.Api.Application.Features.Posts.CountInteraction;
using SharingNote.Api.Application.Features.Posts.CreatePost;
using SharingNote.Api.Application.Features.Posts.DeletePost;
using SharingNote.Api.Application.Features.Posts.GetPost;
using SharingNote.Api.Application.Features.Posts.InteractPost;
using SharingNote.Api.Application.Features.Posts.SearchPosts;
using SharingNote.Api.Application.Features.Posts.UpdatePost;
using SharingNote.Api.Hubs;
using SharingNote.Api.REST.Controllers.Requests.Posts;

namespace SharingNote.Api.REST.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly AuthorizationService _authorizationService;
        private readonly IHubContext<InteractionHub> _hubContext;

        public PostsController(
            ISender sender,
            AuthorizationService authorizationService,
            IHubContext<InteractionHub> hubContext)
        {
            _sender = sender;
            _authorizationService = authorizationService;
            _hubContext = hubContext;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PostDto>> Create([FromBody] CreatePostRequest request)
        {
            var command = new CreatePostCommand(
                request.Title,
                request.Content,
                request.TagIds,
                User.GetUserId());
            var response = await _sender.Send(command);
            return this.ToActionResult(response);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<IEnumerable<PostDto>>>> Search([FromQuery] SearchPostsQuery query)
        {
            var response = await _sender.Send(query);
            return this.ToActionResult(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> Get(Guid id)
        {
            var response = await _sender.Send(new GetPostQuery(id));
            return this.ToActionResult(response);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, UpdatePostRequest request)
        {
            if (!await _authorizationService.IsAuthorizedToManagePost(User, id))
            {
                return Forbid();
            }

            var command = new UpdatePostCommand(
                id,
                request.Title,
                request.Content,
                request.TagIds);
            var response = await _sender.Send(command);
            return this.ToActionResult(response);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!await _authorizationService.IsAuthorizedToManagePost(User, id))
            {
                return Forbid();
            }

            var response = await _sender.Send(new DeletePostCommand(id));
            return this.ToActionResult(response);
        }

        [HttpPost("{id}/interact")]
        [Authorize]
        public async Task<IActionResult> Interact(Guid id, CreatePostInteractionRequest request)
        {

            var response = await _sender.Send(new InteractPostCommand(
                id,
                User.GetUserId(),
                request.InteractionType));

            var res = await _sender.Send(new CountInteractionQuery(id, request.InteractionType));

            if (res.IsSuccess)
            {
                var interactionCount = res.Value;
                await _hubContext.Clients.All.SendAsync("UpdatePostInteraction", interactionCount);
            }

            return this.ToActionResult(response);
        }
    }
}
