namespace SharingNote.Api.Application.Features.Posts.InteractPost
{
    internal class InteractPostCommandHandler(AppDbContext dbContext)
        : ICommandHandler<InteractPostCommand>
    {
        public async Task<Result> Handle(InteractPostCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var post = await dbContext.Posts
                        .Where(x => x.Id == command.PostId)
                        .FirstOrDefaultAsync();

                if (post == null)
                {
                    return Result.NotFound();
                }


                var interactionType = Enum.Parse<InteractionType>(command.InteractionType, true);

                var interaction = await dbContext.PostInteractions
                    .Where(x => x.PostId == command.PostId
                        && x.UserId == command.UserId
                        && x.Type == interactionType)
                    .FirstOrDefaultAsync();

                if (interaction == null)
                {
                    interaction = new PostInteraction(command.PostId, command.UserId, interactionType);
                    dbContext.PostInteractions.Add(interaction);

                }
                else
                {
                    dbContext.PostInteractions.Remove(interaction);
                }

                await dbContext.SaveChangesAsync();

                return Result.NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result.CriticalError("A concurrency error occurred. Please try again.");
            }
        }
    }
}
