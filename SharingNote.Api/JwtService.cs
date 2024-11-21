using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SharingNote.Api;

public record TokenResponse(string AccessToken, string RefreshToken);
public class JwtService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;

    public JwtService(
        UserManager<AppUser> userManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    private string GenerateAccessToken(List<Claim> claims)
    {
        var now = DateTime.UtcNow;
        var secret = _configuration["JwtAuthSettings:Secret"]!;
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));

        var expirationInMinutes = int.Parse(_configuration["JwtAuthSettings:AccessTokenExpiresInMinutes"]!);

        var jwtSecurityToken = new JwtSecurityToken(
            notBefore: now,
            claims: claims,
            expires: now.Add(TimeSpan.FromMinutes(expirationInMinutes)),
            audience: _configuration["JwtAuthSettings:Audience"]!,
        issuer: _configuration["JwtAuthSettings:Issuer"]!,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }
    public async Task<TokenResponse> GenerateAuthResponseAsync(AppUser user)
    {

        var userClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        var roles = await _userManager.GetRolesAsync(user);
        userClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var accessToken = GenerateAccessToken(userClaims);
        var refreshToken = GenerateRefreshToken();

        var expirationInDays = int.Parse(_configuration["JwtAuthSettings:RefreshTokenExpiresInDays"]!);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(expirationInDays);

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return new TokenResponse(string.Empty, string.Empty);
        }

        return new TokenResponse(accessToken, refreshToken);
    }

    public ClaimsPrincipal GetPrincipalFromAccessToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JwtAuthSettings:Secret"]!)
            ),
            ValidIssuer = _configuration["JwtAuthSettings:Issuer"]!,
            ValidAudience = _configuration["JwtAuthSettings:Audience"]!
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtToken ||
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            return principal;
        }
        catch (Exception ex)
        {
            throw new SecurityTokenException("Token validation failed", ex);
        }
    }

    public async Task<TokenResponse> RefreshTokenAsync(string accessToken, string refreshToken)
    {
        var principal = GetPrincipalFromAccessToken(accessToken);
        var email = principal.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryDate <= DateTime.UtcNow)
        {
            return new TokenResponse(string.Empty, string.Empty);
        }

        return await GenerateAuthResponseAsync(user);
    }
}
