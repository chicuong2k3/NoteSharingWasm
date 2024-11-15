using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharingNote.Api.Domain;

namespace SharingNote.Api.Infrastructure.Database.EFCore
{
    public sealed class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Content)
                .IsRequired()
                .HasMaxLength(1000);

            builder.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder.HasOne<Comment>()
                .WithMany()
                .HasForeignKey(x => x.ParentCommentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
