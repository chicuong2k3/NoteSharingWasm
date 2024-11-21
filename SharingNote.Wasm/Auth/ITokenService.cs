namespace SharingNote.Wasm.Auth;

public interface ITokenService
{
    Task<string> TryRefreshTokenAsync();
}
