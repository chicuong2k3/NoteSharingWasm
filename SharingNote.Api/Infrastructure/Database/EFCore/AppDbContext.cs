using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SharingNote.Api.Domain;

namespace SharingNote.Api.Infrastructure.Database.EFCore
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }



        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Domain.Tag> Tags { get; set; }
        public DbSet<PostInteraction> PostInteractions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new TagConfiguration());
            modelBuilder.ApplyConfiguration(new PostInteractionConfiguration());

            modelBuilder.Entity<AppUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole<Guid>>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");

            modelBuilder.Entity<IdentityUserLogin<Guid>>().HasKey(x => new
            {
                x.LoginProvider,
                x.ProviderKey,
                x.UserId
            });
            modelBuilder.Entity<IdentityUserToken<Guid>>().HasKey(x => new
            {
                x.UserId,
                x.LoginProvider,
                x.Name
            });
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasKey(x => new
            {
                x.UserId,
                x.RoleId
            });
        }

    }
}
