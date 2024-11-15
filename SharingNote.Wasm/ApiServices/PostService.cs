
using SharedKernel.Contracts;
using System.Net.Http.Json;

namespace SharingNote.Wasm.ApiServices
{
    class PostService : IPostService
    {
        private readonly HttpClient _httpClient;

        public PostService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<CreatePostResponse?> CreatePostAsync(CreatePostRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("/posts", request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CreatePostResponse>();
            }

            return null;
        }

        public async Task<HttpResponseMessage> DeletePostAsync(Guid id)
        {
            return await _httpClient.DeleteAsync($"/posts/{id}");
        }

        public async Task<PostResponse?> GetPostAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"/posts/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<PostResponse>();
        }


        public async Task<GetPostsResponse?> GetPostsAsync(int pageNumber, int pageSize, string? queryText, string? orderBy, List<Guid>? tagIds, Guid? userId)
        {
            var queryParams = new List<string>
            {
                $"pageNumber={pageNumber}",
                $"pageSize={pageSize}"
            };

            if (!string.IsNullOrEmpty(queryText))
            {
                queryParams.Add($"queryText={Uri.EscapeDataString(queryText)}");
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                queryParams.Add($"orderBy={Uri.EscapeDataString(orderBy)}");
            }

            if (tagIds != null && tagIds.Any())
            {
                queryParams.AddRange(tagIds.Select(tagId => $"tagIds={tagId.ToString()}"));
            }

            if (userId.HasValue)
            {
                queryParams.Add($"userId={userId}");
            }

            var queryString = string.Join("&", queryParams);
            var response = await _httpClient.GetAsync($"/posts?{queryString}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<GetPostsResponse>();
        }


        public async Task<HttpResponseMessage> UpdatePostAsync(Guid id, UpdatePostRequest request)
        {
            return await _httpClient.PutAsJsonAsync($"/posts/{id}", request);
        }
    }
}
