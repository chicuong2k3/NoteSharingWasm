using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SharingNote.Wasm.ApiServices;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace SharingNote.Wasm.Auth
{
    /// <summary>
    /// Custom authentication state provider to manage user authentication state.
    /// </summary>
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;
        public CustomAuthStateProvider(
            HttpClient httpClient,
            ILocalStorageService localStorage,
            IUserService userService)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var claimsPrincipal = new ClaimsPrincipal();
            var accessToken = await _localStorage.GetItemAsync<string>("accessToken");

            if (!string.IsNullOrEmpty(accessToken))
            {
                claimsPrincipal = ParseClaimsFromAccessToken(accessToken);
                _httpClient.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            return new AuthenticationState(claimsPrincipal);
        }
        public async Task NotifyUserLoginAsync(TokenResponse tokenResponse)
        {
            await _localStorage.SetItemAsync("accessToken", tokenResponse.AccessToken);
            await _localStorage.SetItemAsync("refreshToken", tokenResponse.RefreshToken);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

            var claimsPrincipal = ParseClaimsFromAccessToken(tokenResponse.AccessToken);

            var authState = Task.FromResult(new AuthenticationState(claimsPrincipal));
            NotifyAuthenticationStateChanged(authState);
        }

        public async Task NotifyUserLogoutAsync()
        {
            await _localStorage.RemoveItemAsync("accessToken");
            await _localStorage.RemoveItemAsync("refreshToken");
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

    }


}
