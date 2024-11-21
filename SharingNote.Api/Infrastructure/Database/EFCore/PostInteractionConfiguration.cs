using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SharingNote.Api.Infrastructure.Database.EFCore
{
    public sealed class PostInteractionConfiguration : IEntityTypeConfiguration<PostInteraction>
    {
        public void Configure(EntityTypeBuilder<PostInteraction> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (InteractionType)Enum.Parse(typeof(InteractionType), v)
                );

            builder.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId);
        }
    }
}
