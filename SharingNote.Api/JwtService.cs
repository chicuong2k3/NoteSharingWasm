using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SharingNote.Api
{
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
        public async Task<Result<string>> AuthenticateAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                return Result.Error("Email or password is incorrect.");
            }

            var tokenResponse = await GenerateAuthResponse(user.Email!);

            if (tokenResponse == null)
            {
                return Result.CriticalError();
            }

            return Result.Success(tokenResponse);
        }
        public async Task<string> GenerateAuthResponse(string email)
        {
            var now = DateTime.UtcNow;
            var secret = _configuration["JwtAuthSettings:Secret"]!;
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return string.Empty;
            }

            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var expirationInMinutes = int.Parse(_configuration["JwtAuthSettings:ExpiresInMinutes"]!);

            var jwtSecurityToken = new JwtSecurityToken(
                notBefore: now,
                claims: userClaims,
                expires: now.Add(TimeSpan.FromMinutes(expirationInMinutes)),
                audience: _configuration["JwtAuthSettings:Audience"]!,
                issuer: _configuration["JwtAuthSettings:Issuer"]!,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}
