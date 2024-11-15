using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authentication.Extensions;
using SharingNote.Api.Application.Features.Users.GetUser;
using SharingNote.Api.Application.Features.Users.RegisterUser;
using SharingNote.Api.Application.Features.Users.UpdateUserInfo;
using SharingNote.Api.Controllers.Requests.Users;


namespace SharingNote.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ISender _sender;

        public UsersController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<RegisterUserResponse>> Register([FromBody] RegisterUserCommand command)
        {
            var response = await _sender.Send(command);
            return this.ToActionResult(response);
        }

        [HttpGet("profile")]
        public async Task<ActionResult<GetUserResponse>> Get()
        {
            var userId = User.GetUserId();
            var response = await _sender.Send(new GetUserQuery(userId));
            return this.ToActionResult(response);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<GetUserResponse>> GetById(Guid id)
        {
            var response = await _sender.Send(new GetUserQuery(id));
            return this.ToActionResult(response);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> Update(UpdateUserInfoRequest request)
        {
            var command = new UpdateUserInfoCommand(User.GetUserId(), request.DisplayName, request.Avatar);
            var response = await _sender.Send(command);
            return this.ToActionResult(response);
        }

    }


}
