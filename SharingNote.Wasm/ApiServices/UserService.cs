using System.Net.Http.Json;

namespace SharingNote.Wasm.ApiServices
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<HttpResponseMessage> RegisterUserAsync(string email, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("/users/register", new
            {
                Email = email,
                Password = password
            });

            return response!;
        }

        public async Task<GetUserResponse?> GetUserInfoAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/users/profile");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<GetUserResponse>();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<GetUserResponse?> GetUserByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"/users/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<GetUserResponse>();
        }

        public async Task<HttpResponseMessage> UpdateUserAsync(string displayName, string avatar)
        {
            var response = await _httpClient.PutAsJsonAsync("/users/profile", new
            {
                DisplayName = displayName,
                Avatar = avatar
            });

            return response;
        }

        public async Task<bool> CheckUserExistAsync(string email)
        {
            var response = await _httpClient.GetAsync($"/users/{email}/exists");

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return await response.Content.ReadFromJsonAsync<bool>();
        }

        public async Task<HttpResponseMessage> SendResetPasswordOtp(string email)
        {
            return await _httpClient.PostAsJsonAsync("/otp/send", new
            {
                Email = email,
                EmailSubject = "Quên mật khẩu"
            });
        }

        public async Task<HttpResponseMessage> ResetPassword(string email, string otp, string newPassword)
        {
            return await _httpClient.PostAsJsonAsync("/users/reset-password", new
            {
                Email = email,
                Otp = otp,
                NewPassword = newPassword
            });
        }
    }
}
