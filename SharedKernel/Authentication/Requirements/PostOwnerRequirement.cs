using Microsoft.AspNetCore.Authorization;
using SharedKernel.Authentication.Extensions;
using SharedKernel.Contracts;

namespace SharedKernel.Authentication.Requirements;

public class PostOwnerRequirement : IAuthorizationRequirement
{
}

public class PostOwnerAuthorizationHandler : AuthorizationHandler<PostOwnerRequirement, Guid>
{
    private readonly IReadPostService _postService;

    public PostOwnerAuthorizationHandler(IReadPostService postService)
    {
        _postService = postService;
    }
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PostOwnerRequirement requirement,
        Guid postId)
    {
        var result = await _postService.GetPostAsync(postId);

        if (result == null)
        {
            context.Fail();
            return;
        }

        var postOwnerId = result.UserId;
        var userId = context.User.GetUserId();

        if (postOwnerId != userId)
        {
            context.Fail();
            return;
        }

        context.Succeed(requirement);
    }
}
