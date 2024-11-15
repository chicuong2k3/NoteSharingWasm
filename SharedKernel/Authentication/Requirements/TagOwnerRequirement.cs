using Microsoft.AspNetCore.Authorization;
using SharedKernel.Authentication.Extensions;
using SharedKernel.Contracts;

namespace SharedKernel.Authentication.Requirements;

public class TagOwnerRequirement : IAuthorizationRequirement
{
}

public class TagOwnerAuthorizationHandler : AuthorizationHandler<TagOwnerRequirement, Guid>
{
    private readonly IReadTagService _tagService;

    public TagOwnerAuthorizationHandler(IReadTagService tagService)
    {
        _tagService = tagService;
    }
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        TagOwnerRequirement requirement,
        Guid tagId)
    {

        var result = await _tagService.GetTagAsync(tagId);

        if (result == null)
        {
            context.Fail();
            return;
        }

        var tagOwnerId = result.UserId;

        if (tagOwnerId != context.User.GetUserId())
        {
            context.Fail();
            return;
        }

        context.Succeed(requirement);
    }
}
