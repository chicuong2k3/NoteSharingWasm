using SharingNote.Api.Application.Features.Tags.GetTags;

namespace SharingNote.Api.Application.Features.Posts.CreatePost
{
    internal class CreatePostCommandHandler(AppDbContext dbContext)
        : ICommandHandler<CreatePostCommand, CreatePostResponse>
    {
        public async Task<Result<CreatePostResponse>> Handle(CreatePostCommand command, CancellationToken cancellationToken)
        {
            var tags = dbContext.Tags
                .Where(x => command.TagIds.Contains(x.Id))
                .ToList();

            var post = new Post(command.Title, command.Content, tags, command.UserId);

            dbContext.Posts.Add(post);
            await dbContext.SaveChangesAsync();

            return Result.Created(post).Map(x => new CreatePostResponse(
                x.Id,
                x.Title,
                x.Content,
                x.Tags.Select(t => new TagDto(t.Id, t.Name, t.UserId)).ToList(),
                x.PublicationDate));
        }
    }
}
