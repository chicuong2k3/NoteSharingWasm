namespace SharingNote.Api.REST.Controllers.Requests.Auth;

public record RefreshTokenRequest(string AccessToken, string RefreshToken);