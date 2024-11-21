using SharingNote.Api.Application.Features.Tags;

namespace SharingNote.Api.GraphQL.Types
{
    [ObjectType<TagDto>]
    public static partial class TagDtoType
    {
        static partial void Configure(IObjectTypeDescriptor<TagDto> descriptor)
        {
            descriptor.Field(t => t.Id).Type<NonNullType<UuidType>>();
        }
    }
}
