using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SharingNote.Wasm.ApiServices;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;

namespace SharingNote.Wasm.Auth
{
    /// <summary>
    /// Custom authentication state provider to manage user authentication state.
    /// </summary>
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly IUserService _userService;

        public CustomAuthStateProvider(
            HttpClient httpClient,
            ILocalStorageService localStorage,
            IUserService userService)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _userService = userService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var claimsPrincipal = new ClaimsPrincipal();
            var accessToken = await _localStorage.GetItemAsync<string>("accessToken");

            if (!string.IsNullOrEmpty(accessToken))
            {
                if (IsTokenExpired(accessToken))
                {
                    await _localStorage.RemoveItemAsync("accessToken");
                    return new AuthenticationState(new ClaimsPrincipal());
                }

                claimsPrincipal = ParseClaimsFromAccessToken(accessToken);
                _httpClient.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            return new AuthenticationState(claimsPrincipal);
        }
        public async Task NotifyUserLoginAsync(string accessToken)
        {
            await _localStorage.SetItemAsync("accessToken", accessToken);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var claimsPrincipal = ParseClaimsFromAccessToken(accessToken);

            var authState = Task.FromResult(new AuthenticationState(claimsPrincipal));
            NotifyAuthenticationStateChanged(authState);
        }

        public async Task NotifyUserLogoutAsync()
        {
            await _localStorage.RemoveItemAsync("accessToken");
            _httpClient.DefaultRequestHeaders.Authorization = null;

            var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));
            NotifyAuthenticationStateChanged(authState);
        }

        private ClaimsPrincipal ParseClaimsFromAccessToken(string accessToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var identity = new ClaimsIdentity();

                if (tokenHandler.CanReadToken(accessToken))
                {
                    var jwtSecurityToken = tokenHandler.ReadJwtToken(accessToken);
                    identity = new ClaimsIdentity(jwtSecurityToken.Claims, Constants.CustomAuthScheme);
                }

                return new ClaimsPrincipal(identity);
            }
            catch
            {
                return new ClaimsPrincipal(new ClaimsIdentity());
            }
        }

        private bool IsTokenExpired(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            return jwtToken.ValidTo < DateTime.UtcNow;
        }
    }


}
