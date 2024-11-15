using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using SharingNote.Wasm.Auth;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SharingNote.Wasm.ApiServices;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ILocalStorageService _localStorage;
    private readonly IJSRuntime _jSRuntime;
    private readonly IConfiguration _configuration;

    public AuthService(HttpClient httpClient,
        AuthenticationStateProvider authenticationStateProvider,
        ILocalStorageService localStorage,
        IJSRuntime jSRuntime,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _authenticationStateProvider = authenticationStateProvider;
        _localStorage = localStorage;
        _jSRuntime = jSRuntime;
        _configuration = configuration;
    }

    public async Task<HttpResponseMessage> LoginAsync(string email, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("/auth/login", new
        {
            email,
            password
        });

        if (response.IsSuccessStatusCode)
        {
            var accessToken = await response.Content.ReadAsStringAsync();
            await ((CustomAuthStateProvider)_authenticationStateProvider).NotifyUserLoginAsync(accessToken);
        }

        return response;
    }

    public async Task LogoutAsync()
    {
        await ((CustomAuthStateProvider)_authenticationStateProvider).NotifyUserLogoutAsync();
    }

    public async Task<bool> LoginWithGoogleAsync()
    {
        try
        {
            // Gọi hàm googleSignIn từ JSInterop
            var idToken = await _jSRuntime.InvokeAsync<string>("googleSignIn", _configuration["Google:ClientId"]);

            if (string.IsNullOrEmpty(idToken))
            {
                Console.WriteLine("Id token is not valid.");
                return false;
            }

            // Gửi ID Token đến API backend để lấy access token
            var response = await _httpClient.PostAsJsonAsync("/auth/google", idToken);
            var result = await response.Content.ReadFromJsonAsync<GoogleLoginResponse>();
            if (result != null && !string.IsNullOrEmpty(result.Result))
            {
                var accessToken = result.Result;
                await _localStorage.SetItemAsync("accessToken", accessToken);
                await ((CustomAuthStateProvider)_authenticationStateProvider).NotifyUserLoginAsync(accessToken);
                return true;
            }
            else
            {
                Console.WriteLine("Login failed: No access token received.");

                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }

}

public class GoogleLoginResponse
{
    public string Result { get; set; } = string.Empty;
    public int Id { get; set; }
    public string Exception { get; set; } = string.Empty;
    public int Status { get; set; }
}

