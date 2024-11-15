using SharingNote.Api.Infrastructure.Database.EFCore;

namespace SharingNote.Api.Application.Features.Comments.DeleteComment
{
    internal class DeleteCommentCommandHandler(AppDbContext dbContext)
        : ICommandHandler<DeleteCommentCommand>
    {
        public async Task<Result> Handle(DeleteCommentCommand command, CancellationToken cancellationToken)
        {
            var comment = await dbContext.Comments.FindAsync(command.Id);

            if (comment == null)
            {
                return Result.NotFound();
            }

            dbContext.Comments.Remove(comment);
            await dbContext.SaveChangesAsync();

            return Result.NoContent();
        }
    }
}
