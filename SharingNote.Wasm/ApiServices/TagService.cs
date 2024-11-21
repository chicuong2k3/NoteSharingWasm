using SharedKernel.Contracts;
using System.Net.Http;
using System.Net.Http.Json;

namespace SharingNote.Wasm.ApiServices
{
    class TagService : ITagService
    {
        private readonly HttpClient _httpClient;

        public TagService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> CreateTagAsync(CreateTagRequest request)
        {
            return await _httpClient.PostAsJsonAsync("/tags", request);
        }

        public async Task<HttpResponseMessage> DeleteTagAsync(Guid id)
        {
            return await _httpClient.DeleteAsync($"/tags/{id}");
        }

        public async Task<TagResponse?> GetTagAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"/tags/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<TagResponse>();
        }

        public async Task<IEnumerable<TagResponse>> GetTagsAsync()
        {
            var response = await _httpClient.GetAsync("/tags");

            if (!response.IsSuccessStatusCode)
            {
                return [];
            }

            return await response.Content.ReadFromJsonAsync<IEnumerable<TagResponse>>() ?? [];
        }
    }
}
