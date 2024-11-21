using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authentication.Extensions;
using SharingNote.Api.Application.Features.Users.ChangePassword;
using SharingNote.Api.Application.Features.Users.CheckUserExists;
using SharingNote.Api.Application.Features.Users.GetUser;
using SharingNote.Api.Application.Features.Users.RegisterUser;
using SharingNote.Api.Application.Features.Users.UpdateUserInfo;
using SharingNote.Api.Application.Services;
using SharingNote.Api.REST.Controllers.Requests.Users;


namespace SharingNote.Api.REST.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IOtpService _otpService;

        public UsersController(ISender sender, IOtpService otpService)
        {
            _sender = sender;
            _otpService = otpService;
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

        [HttpGet("{email}/exists")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> Exists(string email)
        {
            var response = await _sender.Send(new CheckUserExistsQuery(email));
            return this.ToActionResult(response);
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {

            var res = await _sender.Send(new CheckUserExistsQuery(request.Email));

            if (!res.IsSuccess || res.Value == false)
            {
                return BadRequest();
            }

            var otpValid = await _otpService.ValidateOtpAsync(request.Email, request.Otp);


            if (!otpValid)
            {
                return BadRequest();
            }

            var command = new ChangeUserPasswordCommand(request.Email, request.NewPassword);
            var response = await _sender.Send(command);

            return this.ToActionResult(response);
        }
    }


}
