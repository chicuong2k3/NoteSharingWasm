using SharingNote.Api.Application.Features.Users.RegisterUser;

namespace SharingNote.Api.Infrastructure.Database.EFCore
{
    public static class DbSeeder
    {
        public static async void SeedDatabase(this IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var sender = scope.ServiceProvider.GetRequiredService<ISender>();

                if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
                {
                    dbContext.Database.Migrate();
                }

                if (!dbContext.Users.Any())
                {
                    var result = await sender.Send(new RegisterUserCommand("admin123@gmail.com", "admin123"));

                    if (result.IsSuccess)
                    {
                        if (!dbContext.Tags.Any())
                        {
                            var tag1 = new Domain.Tag(".NET", result.Value.UserId);
                            var tag2 = new Domain.Tag("Python", result.Value.UserId);
                            var tag3 = new Domain.Tag("Java", result.Value.UserId);

                            var tags = new List<Domain.Tag>()
                            {
                                tag2 , tag1, tag3
                            };

                            dbContext.AddRange(tags);
                            dbContext.SaveChanges();
                        }

                        if (!dbContext.Posts.Any())
                        {
                            var faker = new Bogus.Faker();
                            var tags = dbContext.Tags.ToList();

                            for (int i = 0; i < 20; i++)
                            {
                                var random = new Random();
                                var randomTags = new List<Domain.Tag>();
                                var randomCount = random.Next(4);

                                for (int j = 0; j < randomCount; j++)
                                {
                                    var index = random.Next(3);
                                    randomTags.Add(tags[index]);
                                }
                                dbContext.Posts.Add(new Post(faker.Commerce.ProductName(), faker.Lorem.Text(), randomTags, result.Value.UserId));
                            }

                            dbContext.SaveChanges();
                        }
                    }
                }




            }
        }
    }
}
