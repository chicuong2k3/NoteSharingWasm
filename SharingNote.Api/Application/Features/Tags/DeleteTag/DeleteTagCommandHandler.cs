using SharingNote.Api.Infrastructure.Database.EFCore;

namespace SharingNote.Api.Application.Features.Tags.DeleteTag
{
    internal class DeleteTagCommandHandler(AppDbContext dbContext)
        : ICommandHandler<DeleteTagCommand>
    {
        public async Task<Result> Handle(DeleteTagCommand command, CancellationToken cancellationToken)
        {
            var tag = await dbContext.Tags.FindAsync(command.TagId);

            if (tag == null)
            {
                return Result.NotFound();
            }

            dbContext.Tags.Remove(tag);
            await dbContext.SaveChangesAsync();

            return Result.NoContent();
        }
    }
}
