using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharingNote.Api.Domain;

namespace SharingNote.Api.Infrastructure.Database.EFCore
{
    public sealed class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Content)
                .IsRequired();

            builder.HasMany<Comment>()
                .WithOne()
                .HasForeignKey(x => x.PostId);

            builder.HasMany(x => x.Tags)
                .WithMany(x => x.Posts)
                .UsingEntity("TagPosts");

            builder.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId);
        }
    }
}
