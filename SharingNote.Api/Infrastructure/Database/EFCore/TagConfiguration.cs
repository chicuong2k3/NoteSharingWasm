using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharingNote.Api.Domain;

namespace SharingNote.Api.Infrastructure.Database.EFCore
{
    public sealed class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId);
        }
    }
}
