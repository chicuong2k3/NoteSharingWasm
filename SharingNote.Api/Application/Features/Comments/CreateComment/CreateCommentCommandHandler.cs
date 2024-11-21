using Akismet;

namespace SharingNote.Api.Application.Features.Comments.CreateComment
{
    internal class CreateCommentCommandHandler(
        AppDbContext dbContext,
        AkismetClient akismetClient)
        : ICommandHandler<CreateCommentCommand, CreateCommentResponse>
    {
        public async Task<Ardalis.Result.Result<CreateCommentResponse>> Handle(CreateCommentCommand command, CancellationToken cancellationToken)
        {
            var postExist = await dbContext.Posts.AnyAsync(x => x.Id == command.PostId);

            if (!postExist)
            {
                return Result.NotFound($"Post with id = {command.PostId} was not found.");
            }

            if (command.ParentId.HasValue)
            {
                var parentCommentExist = await dbContext.Comments.AnyAsync(x => x.Id == command.ParentId);

                if (!parentCommentExist)
                {
                    return Result.NotFound($"Parent comment with id = {command.ParentId} was not found.");
                }
            }


            // check spam
            var checkCommentResult = await akismetClient.CheckCommentAsync(new AkismetComment
            {
                CommentContent = command.Content,
            });

            if (checkCommentResult.IsSpam)
            {
                return Result.Error("Comment is spam.");
            }


            var comment = new Comment(command.PostId, command.UserId, command.Content, command.ParentId);

            dbContext.Comments.Add(comment);
            await dbContext.SaveChangesAsync();

            return Result.Created(comment).Map(x => new CreateCommentResponse(
                x.Id,
                x.PostId,
                x.UserId,
                x.Content,
                x.PostedDate,
                x.LastModifiedDate,
                x.ParentCommentId));
        }
    }
}
