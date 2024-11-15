

namespace SharingNote.Api.Application.Features.Tags.CreateTag
{
    internal class CreateTagCommandHandler(AppDbContext dbContext)
        : ICommandHandler<CreateTagCommand, CreateTagResponse>
    {
        public async Task<Result<CreateTagResponse>> Handle(CreateTagCommand command, CancellationToken cancellationToken)
        {
            var tag = new Tag(command.Name, command.UserId);

            dbContext.Tags.Add(tag);
            await dbContext.SaveChangesAsync();

            return Result.Created(tag).Map(x => new CreateTagResponse(
                x.Id,
                x.Name,
                x.UserId));
        }
    }
}
