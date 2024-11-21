

namespace SharingNote.Api.Application.Features.Tags.CreateTag
{
    internal class CreateTagCommandHandler(AppDbContext dbContext)
        : ICommandHandler<CreateTagCommand, TagDto>
    {
        public async Task<Ardalis.Result.Result<TagDto>> Handle(CreateTagCommand command, CancellationToken cancellationToken)
        {
            var tag = new Domain.Tag(command.Name, command.UserId);

            dbContext.Tags.Add(tag);
            await dbContext.SaveChangesAsync();

            return Result.Created(tag).Map(x => new TagDto(
                x.Id,
                x.Name,
                x.UserId));
        }
    }
}
