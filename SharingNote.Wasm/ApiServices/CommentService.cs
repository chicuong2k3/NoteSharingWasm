
using SharedKernel.Contracts;
using System.Net.Http.Json;

namespace SharingNote.Wasm.ApiServices
{
    class CommentService : ICommentService
    {
        private readonly HttpClient _httpClient;

        public CommentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<CreateCommentResponse?> CreateCommentAsync(CreateCommentRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("comments", request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<CreateCommentResponse>();
        }

        public async Task<HttpResponseMessage> DeleteCommentAsync(Guid commentId)
        {
            return await _httpClient.DeleteAsync($"comments/{commentId}");
        }

        public async Task<CommentResponse?> GetCommentAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"comments/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<CommentResponse>();
        }

        public async Task<GetCommentsResponse?> GetCommentsAsync(Guid postId, int count, Guid? parentId, string? sortDirection, DateTime? timeCursor)
        {
            var queryParams = new List<string>
            {
                $"postId={postId}",
                $"count={count}"
            };

            if (parentId.HasValue)
            {
                queryParams.Add($"parentId={parentId}");
            }

            if (!string.IsNullOrEmpty(sortDirection))
            {
                queryParams.Add($"sortDirection={sortDirection}");
            }

            if (timeCursor.HasValue)
            {
                queryParams.Add($"timeCursor={timeCursor.Value.ToString("o")}");
            }

            var queryString = string.Join("&", queryParams);
            var response = await _httpClient.GetAsync($"comments?{queryString}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<GetCommentsResponse>();
        }

        public async Task<HttpResponseMessage> UpdateCommentAsync()
        {
            return await _httpClient.PutAsync("comments", null);
        }
    }
}
