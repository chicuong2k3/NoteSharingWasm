namespace SharingNote.Wasm.Auth
{
    public interface IAuthService
    {
        Task<HttpResponseMessage> LoginAsync(string email, string password);
        Task LogoutAsync();
        Task<bool> LoginWithGoogleAsync();
    }
}
