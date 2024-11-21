using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using SharingNote.Api.Application.Features.Users.RegisterUser;
using SharingNote.Api.Application.Features.Users.UpdateUserInfo;
using SharingNote.Api.Application.Services;
using SharingNote.Api.REST.Controllers.Requests.Auth;

namespace SharingNote.Api.REST.Controllers;




[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly JwtService _jwtService;
    private readonly ISender _sender;
    private readonly IConfiguration _configuration;
    private readonly IEmailSendingService _emailSendingService;

    public AuthController(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        JwtService jwtService,
        ISender sender,
        IConfiguration configuration,
        IEmailSendingService emailSendingService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtService = jwtService;
        _sender = sender;
        _configuration = configuration;
        _emailSendingService = emailSendingService;
    }

    [HttpPost("/auth/login")]
    public async Task<ActionResult<string>> Login(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return BadRequest("Email or password is incorrect.");
        }

        var tokenResponse = await _jwtService.GenerateAuthResponseAsync(user);

        if (tokenResponse == null)
        {
            return StatusCode(500);
        }

        return Ok(tokenResponse);
    }

    [HttpPost("/auth/google")]
    public async Task<IActionResult> GoogleLogin([FromBody] string idToken)
    {
        var googleTokenValidator = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new List<string> { _configuration["Google:ClientId"]! }
        };

        try
        {
            // 1. Validate Google ID token and retrieve payload
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, googleTokenValidator);
            var email = payload.Email;

            // 2. Check if the user exists
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // Register new user
                var registerResult = await _sender.Send(new RegisterUserCommand(email, "123456"));

                if (!registerResult.IsSuccess)
                {
                    return BadRequest("User registration failed.");
                }

                // Create new claims for the registered user
                user = await _userManager.FindByEmailAsync(email);

                var givenName = payload.GivenName ?? string.Empty;
                var familyName = payload.FamilyName ?? string.Empty;
                var avatar = payload.Picture ?? string.Empty;

                await _sender.Send(new UpdateUserInfoCommand(registerResult.Value.UserId, $"{familyName} {givenName}", avatar));
            }

            // 3. Generate token
            var tokenReponse = _jwtService.GenerateAuthResponseAsync(user!);

            return Ok(tokenReponse);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpPost("/auth/refresh")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
    {
        var tokenResponse = await _jwtService.RefreshTokenAsync(request.AccessToken, request.RefreshToken);

        if (string.IsNullOrEmpty(tokenResponse.AccessToken)
            || string.IsNullOrEmpty(tokenResponse.RefreshToken))
        {
            return BadRequest("Invalid refresh token.");
        }

        return Ok(tokenResponse);
    }

    [HttpPost("/email/send")]
    public async Task<IActionResult> SendEmail(SendEmailRequest request)
    {
        var result = await _emailSendingService.SendEmailAsync(
            request.ToEmail,
            request.ToName,
            request.Subject,
            request.HtmlContent);

        if (!result)
        {
            return BadRequest();
        }

        return Ok();
    }


}
