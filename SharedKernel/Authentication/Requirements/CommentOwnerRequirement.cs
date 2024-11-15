using Microsoft.AspNetCore.Authorization;
using SharedKernel.Authentication.Extensions;
using SharedKernel.Contracts;

namespace SharedKernel.Authentication.Requirements;

public class CommentOwnerRequirement : IAuthorizationRequirement
{
}

public class CommentOwnerAuthorizationHandler : AuthorizationHandler<CommentOwnerRequirement, Guid>
{
    private readonly IReadCommentService _commentService;

    public CommentOwnerAuthorizationHandler(IReadCommentService commentService)
    {
        _commentService = commentService;
    }
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CommentOwnerRequirement requirement,
        Guid commentId)
    {
        var result = await _commentService.GetCommentAsync(commentId);

        if (result == null)
        {
            context.Fail();
            return;
        }

        var commenterId = result.UserId;

        if (commenterId != context.User.GetUserId())
        {
            context.Fail();
            return;
        }

        context.Succeed(requirement);
    }
}
