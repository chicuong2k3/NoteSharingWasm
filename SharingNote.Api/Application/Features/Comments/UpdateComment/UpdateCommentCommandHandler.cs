using SharingNote.Api.Infrastructure.Database.EFCore;

namespace SharingNote.Api.Application.Features.Comments.UpdateComment
{
    internal class UpdateCommentCommandHandler(AppDbContext dbContext)
        : ICommandHandler<UpdateCommentCommand>
    {
        public async Task<Result> Handle(UpdateCommentCommand command, CancellationToken cancellationToken)
        {
            var comment = await dbContext.Comments.FindAsync(command.Id);

            if (comment == null)
            {
                return Result.NotFound();
            }


            comment.Update(command.Content);

            await dbContext.SaveChangesAsync();

            return Result.NoContent();
        }
    }
}
