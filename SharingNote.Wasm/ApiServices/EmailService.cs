
using System.Net.Http.Json;

namespace SharingNote.Wasm.ApiServices
{
    public class EmailService : IEmailService
    {
        private readonly HttpClient _httpClient;

        public EmailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<HttpResponseMessage> SendEmailAsync(EmailRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("/email/send", request);

            return response;
        }
    }
}
