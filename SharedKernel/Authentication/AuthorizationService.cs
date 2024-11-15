using Microsoft.AspNetCore.Authorization;
using SharedKernel.Authentication.Requirements;
using System.Security.Claims;

namespace SharedKernel.Authentication
{
    public class AuthorizationService
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationService(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public async Task<bool> IsAuthorizedToManageComment(ClaimsPrincipal user, Guid commentId)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(user, commentId, PolicyConstants.CanManageComment);
            return authorizationResult.Succeeded;
        }

        public async Task<bool> IsAuthorizedToManageTag(ClaimsPrincipal user, Guid tagId)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(user, tagId, PolicyConstants.CanManageTag);
            return authorizationResult.Succeeded;
        }

        public async Task<bool> IsAuthorizedToManagePost(ClaimsPrincipal user, Guid postId)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(user, postId, PolicyConstants.CanManagePost);
            return authorizationResult.Succeeded;
        }
    }

}
