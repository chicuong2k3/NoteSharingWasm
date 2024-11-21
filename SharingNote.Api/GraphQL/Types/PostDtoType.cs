using SharingNote.Api.Application.Features.Posts;
using SharingNote.Api.Application.Features.Tags;

namespace SharingNote.Api.GraphQL.Types
{
    [ObjectType<PostDto>]
    public static partial class PostDtoType
    {
        static partial void Configure(IObjectTypeDescriptor<PostDto> descriptor)
        {
            descriptor.Field(p => p.Id).Type<NonNullType<UuidType>>();
            descriptor.Field(p => p.Tags).Type<ListType<NonNullType<ObjectType<TagDto>>>>();
        }

        public static async Task<AppUser> GetUser([Parent] PostDto post, AppDbContext dbContext)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == post.UserId);
            return user;
        }
    }
}
