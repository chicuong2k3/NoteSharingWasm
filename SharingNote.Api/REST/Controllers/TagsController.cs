using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authentication;
using SharedKernel.Authentication.Extensions;
using SharingNote.Api.Application.Features.Tags;
using SharingNote.Api.Application.Features.Tags.CreateTag;
using SharingNote.Api.Application.Features.Tags.DeleteTag;
using SharingNote.Api.Application.Features.Tags.GetTag;
using SharingNote.Api.Application.Features.Tags.GetTags;
using SharingNote.Api.REST.Controllers.Requests.Tags;

namespace SharingNote.Api.REST.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly AuthorizationService _authorizationService;

        public TagsController(ISender sender, AuthorizationService authorizationService)
        {
            _sender = sender;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<TagDto>> Create([FromBody] CreateTagRequest request)
        {
            var command = new CreateTagCommand(
                request.Name,
                User.GetUserId());
            var response = await _sender.Send(command);
            return this.ToActionResult(response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagDto>>> GetAll([FromQuery] GetTagsQuery query)
        {
            var response = await _sender.Send(query);
            return this.ToActionResult(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TagDto>> Get(Guid id)
        {
            var response = await _sender.Send(new GetTagQuery(id));
            return this.ToActionResult(response);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!await _authorizationService.IsAuthorizedToManageTag(User, id))
            {
                return Forbid();
            }

            var response = await _sender.Send(new DeleteTagCommand(id));
            return this.ToActionResult(response);
        }


    }
}
