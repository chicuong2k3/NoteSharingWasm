using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Json;

namespace SharingNote.Wasm.Auth;

public class TokenService : ITokenService
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public TokenService(ILocalStorageService localStorage,
        HttpClient httpClient,
        AuthenticationStateProvider authenticationStateProvider)
    {
        _localStorage = localStorage;
        _httpClient = httpClient;
        _authenticationStateProvider = authenticationStateProvider;
    }

    private async Task<string> RefreshTokenAsync()
    {
        var accessToken = await _localStorage.GetItemAsync<string>("accessToken");
        var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");

        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
        {
            await ((CustomAuthStateProvider)_authenticationStateProvider).NotifyUserLogoutAsync();
            return string.Empty;
        }

        var response = await _httpClient.PostAsJsonAsync("/auth/refresh", new { accessToken, refreshToken });
        if (!response.IsSuccessStatusCode)
        {
            await ((CustomAuthStateProvider)_authenticationStateProvider).NotifyUserLogoutAsync();
            return string.Empty;
        }

        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
        if (tokenResponse != null)
        {
            await ((CustomAuthStateProvider)_authenticationStateProvider).NotifyUserLoginAsync(tokenResponse);
            return tokenResponse.AccessToken;
        }

        return string.Empty;
    }

    public async Task<string> TryRefreshTokenAsync()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        var exp = user.FindFirst(c => c.Type.Equals("exp"))?.Value;

        if (long.TryParse(exp, out var expSeconds))
        {
            var expTime = DateTimeOffset.FromUnixTimeSeconds(expSeconds);
            var timeUTC = DateTime.UtcNow;
            var diff = expTime - timeUTC;

            if (diff.TotalMinutes <= 2)
            {
                return await RefreshTokenAsync();
            }
        }

        return string.Empty;
    }
}
