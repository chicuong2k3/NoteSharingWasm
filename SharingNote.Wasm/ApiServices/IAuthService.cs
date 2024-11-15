namespace SharingNote.Wasm.ApiServices
{
    public interface IAuthService
    {
        Task<HttpResponseMessage> LoginAsync(string email, string password);
        Task LogoutAsync();
        Task<bool> LoginWithGoogleAsync();
    }
}
