using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authentication;
using SharedKernel.Authentication.Extensions;
using SharingNote.Api.Application.Features.Posts.CreatePost;
using SharingNote.Api.Application.Features.Posts.DeletePost;
using SharingNote.Api.Application.Features.Posts.GetPost;
using SharingNote.Api.Application.Features.Posts.SearchPosts;
using SharingNote.Api.Application.Features.Posts.UpdatePost;
using SharingNote.Api.Controllers.Requests.Posts;

namespace SharingNote.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly AuthorizationService _authorizationService;

        public PostsController(ISender sender, AuthorizationService authorizationService)
        {
            _sender = sender;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CreatePostResponse>> Create([FromBody] CreatePostRequest request)
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
        public async Task<ActionResult<GetPostResponse>> Get(Guid id)
        {
            var response = await _sender.Send(new GetPostQuery(id));
            return this.ToActionResult(response);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, UpdatePostRequest request)
        {
            if (!(await _authorizationService.IsAuthorizedToManagePost(User, id)))
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
            if (!(await _authorizationService.IsAuthorizedToManagePost(User, id)))
            {
                return Forbid();
            }

            var response = await _sender.Send(new DeletePostCommand(id));
            return this.ToActionResult(response);
        }


    }
}
