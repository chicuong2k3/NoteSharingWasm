using SharingNote.Api.Application.Features.Tags;

namespace SharingNote.Api.Application.Features.Posts.CreatePost
{
    internal class CreatePostCommandHandler(AppDbContext dbContext)
        : ICommandHandler<CreatePostCommand, PostDto>
    {
        public async Task<Ardalis.Result.Result<PostDto>> Handle(CreatePostCommand command, CancellationToken cancellationToken)
        {
            var tags = dbContext.Tags
                .Where(x => command.TagIds.Contains(x.Id))
                .ToList();

            var post = new Post(command.Title, command.Content, tags, command.UserId);

            dbContext.Posts.Add(post);
            await dbContext.SaveChangesAsync();

            return Result.Created(post).Map(x => new PostDto(
                x.Id,
                x.Title,
                x.Content,
                x.Tags.Select(t => new TagDto(t.Id, t.Name, t.UserId)).ToList(),
                x.PublicationDate,
                x.UserId,
                x.Interactions.Select(i => new PostInteractionDto(
                    i.Id,
                    i.UserId,
                    i.Type.ToString(),
                    i.CreatedAt
                )).ToList()
             ));
        }
    }
}
