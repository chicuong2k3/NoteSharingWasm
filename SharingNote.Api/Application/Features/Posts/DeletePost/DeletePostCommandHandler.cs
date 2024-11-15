namespace SharingNote.Api.Application.Features.Posts.DeletePost
{
    internal class DeletePostCommandHandler(AppDbContext dbContext)
        : ICommandHandler<DeletePostCommand>
    {
        public async Task<Result> Handle(DeletePostCommand command, CancellationToken cancellationToken)
        {
            var post = await dbContext.Posts.FindAsync(command.PostId);

            if (post == null)
            {
                return Result.NotFound();
            }

            dbContext.Posts.Remove(post);
            await dbContext.SaveChangesAsync();

            return Result.NoContent();
        }
    }
}
