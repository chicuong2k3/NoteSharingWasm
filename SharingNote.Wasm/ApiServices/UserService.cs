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
            var response = await _httpClient.GetAsync("/users/profile");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<GetUserResponse>();
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
    }
}
