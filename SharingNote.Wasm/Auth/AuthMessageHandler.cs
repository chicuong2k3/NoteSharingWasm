using System.Net.Http.Headers;

namespace SharingNote.Wasm.Auth;

public class AuthMessageHandler : DelegatingHandler
{
    private readonly ITokenService _tokenService;

    public AuthMessageHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var absolutePath = request.RequestUri?.AbsolutePath ?? string.Empty;

        if (!absolutePath.Contains("auth"))
        {
            var accessToken = await _tokenService.TryRefreshTokenAsync();
            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }

}
