using Ardalis.Result.AspNetCore;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using SharingNote.Api.Application.Features.Users.RegisterUser;
using SharingNote.Api.Application.Features.Users.UpdateUserInfo;

namespace SharingNote.Api.Controllers;

public record LoginRequest(string Email, string Password);

[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly JwtService _jwtService;
    private readonly ISender _sender;
    private readonly IConfiguration _configuration;

    public AuthController(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        JwtService jwtService,
        ISender sender,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtService = jwtService;
        _sender = sender;
        _configuration = configuration;
    }

    [HttpPost("/auth/login")]
    public async Task<ActionResult<string>> Login(LoginRequest request)
    {
        var response = await _jwtService.AuthenticateAsync(request.Email, request.Password);
        return this.ToActionResult(response);
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

            // 3. Generate JWT token
            var token = _jwtService.GenerateAuthResponse(email);

            return Ok(token);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

}
