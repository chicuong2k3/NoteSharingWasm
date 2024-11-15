namespace SharingNote.Api.Application.Features.Posts.UpdatePost
{
    internal class UpdatePostCommandHandler(AppDbContext dbContext)
        : ICommandHandler<UpdatePostCommand>
    {
        public async Task<Result> Handle(UpdatePostCommand command, CancellationToken cancellationToken)
        {
            var post = await dbContext.Posts.FindAsync(command.Id, cancellationToken);

            if (post == null)
            {
                return Result.NotFound();
            }

            var tags = dbContext.Tags
                .Where(x => command.TagIds.Contains(x.Id))
                .ToList();

            post.Update(command.Title, command.Content, tags);

            await dbContext.SaveChangesAsync();

            return Result.NoContent();
        }
    }
}
